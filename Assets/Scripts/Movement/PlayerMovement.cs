using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;        // Anlýk hareket hýzý
    public float walkSpeed;        // Yürüyüþ hýzý
    public float sprintSpeed;      // Sprint (koþu) hýzý

    [Header("Ground Check")]
    public float playerHeight;     // Oyuncunun yüksekliði
    public LayerMask whatIsGround; // Yere hangi layer'larýn "zemin" kabul edileceði
    public float groundDrag;       // Yerdeyken uygulanan sürtünme
    public bool grounded;          // Oyuncu yerde mi?

    [Header("Jump")]
    public float jumpForce;        // Zýplama kuvveti
    public float jumpCoolDown;     // Zýplamalar arasýndaki bekleme süresi
    public float airMultiplier;    // Havadayken kontrol oraný
    public bool readyToJump;       // Oyuncu zýplamaya hazýr mý?

    [Header("Crouching")]
    public float crouchSpeed;      // Çömelme hýzý
    public float crouchYScale;     // Çömelme boy yüksekliði
    private float startYScale;     // Baþlangýç boy yüksekliði

    [Header("Slope Handling")]
    public float maxSlopeAngle;    // Çýkýlabilecek en dik eðim açýsý
    private RaycastHit slopeHit;   // Eðimin bilgisi
    private bool exitingSlope;     // Eðimden çýkýyor mu?
    public Transform orientation;  // Oyuncunun yönünü referans alacak transform

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;       // Zýplama tuþu
    public KeyCode sprintKey = KeyCode.LeftShift; // Sprint tuþu
    public KeyCode crouchKey = KeyCode.LeftControl; // Çömelme tuþu

    float horizontalInput;         // Yatay input (A-D)
    float verticalInput;           // Dikey input (W-S)

    Vector3 moveDirection;         // Hareket yönü

    Rigidbody rb;                  // Fizik bileþeni

    public MovementState state;    // Oyuncunun hareket durumu

    // Hareket durumlarýný temsil eden enum
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody alýnýr
        rb.freezeRotation = true;       // Fiziksel dönme engellenir

        startYScale = transform.localScale.y; // Baþlangýç boyu kaydedilir
    }

    private void Update()
    {
        // Oyuncu yerde mi kontrol et (Raycast ile aþaðý bakýlýr)
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();       // Inputlarý al
        SpeedControl();  // Hýzý sýnýrla
        StateHandler();  // Hareket durumunu güncelle

        // Drag deðerini güncelle
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer(); // Fiziksel hareketi uygula
    }

    private void MyInput()
    {
        // Inputlarý oku
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Zýplama
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        // Çömelme baþlat
        if (grounded && Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // Oyuncuyu aþaðý it
        }

        // Çömelmeyi býrak
        if (grounded && Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Çömelme durumu
        if (grounded && Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // Sprint durumu
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        // Yürüyüþ durumu
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // Havada olma durumu
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // Hareket yönü hesaplanýr
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Eðimde hareket
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            // Yokuþ çýkarken yukarý doðru fazla ivme olmamasý için aþaðý kuvvet uygula
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // Normal düz zemin hareketi
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // Havada hareket
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // Eðimdeyken gravity kapatýlýr
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // Eðimde hýz kontrolü
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            // Yatay (XZ) hýz kontrolü
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // Önce dikey hýz sýfýrlanýr (daha kontrollü zýplama için)
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Yukarýya zýplama kuvveti verilir
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        // Oyuncunun eðimde olup olmadýðýný kontrol eder
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        // Hareket yönünü eðime uygun þekilde projekte eder
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

   
}
