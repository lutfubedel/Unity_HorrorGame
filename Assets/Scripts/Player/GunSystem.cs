using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunSystem : MonoBehaviour
{
    [Header("Gun Stats")]
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int maganizeSize, bulletsPerTap;
    public bool allowButtonHold;
    public int bulletsLeft, bulletsShot;
    public int totalAmmo;
    public TMP_Text ammoText;
    public ParticleSystem muzzleFlash;

    [Header("Bools")]
    public bool shooting;
    public bool readyToShoot;
    public bool reloading;

    [Header("Referances")]
    public Camera fpsCam;
    public Transform attackPoint;
    public Animator anim;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public Image crosshair;

    [Header("Camera Shake")]
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;

    [Header("Audio Clips")]
    public AudioClip gunShotSound;
    public AudioClip relaodSound;
    public AudioClip takeSound;

    [Header("Impact Effects")]
    public GameObject bloodEffect; 
    public GameObject stoneEffect;

    AudioSource source;

    private void Start()
    {
        bulletsLeft = maganizeSize;
        readyToShoot = true;

        source = GetComponent<AudioSource>();
        source.PlayOneShot(takeSound);
    }

    private void Update()
    {
        MyInput();
        CrossColor();
        ammoText.text = bulletsLeft.ToString() + " / " + totalAmmo.ToString();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < maganizeSize && !reloading && totalAmmo > 0) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }





    private void Shoot()
    {
        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit");
                rayHit.collider.GetComponent<EnemyManager>().TakeDamage(damage);

                GameObject impactGO = Instantiate(bloodEffect, rayHit.point, Quaternion.LookRotation(rayHit.normal));
                Destroy(impactGO, 1f);
            }
            else
            {
                GameObject impactGO = Instantiate(stoneEffect, rayHit.point, Quaternion.LookRotation(rayHit.normal));
                Destroy(impactGO, 1f);
            }
        }

        camShake.Shake(camShakeDuration, camShakeMagnitude);

        bulletsLeft--;
        bulletsShot--;

        anim.SetTrigger("Shot");

        muzzleFlash.Play();
        source.PlayOneShot(gunShotSound);

        Invoke(nameof(ResetShot), timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        source.PlayOneShot(relaodSound);
        anim.SetBool("Reload", true);
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {

        int bulletsNeeded = maganizeSize - bulletsLeft;

        int bulletsToLoad = (totalAmmo >= bulletsNeeded) ? bulletsNeeded : totalAmmo;

        totalAmmo -= bulletsToLoad; 
        bulletsLeft += bulletsToLoad;

        reloading = false;
        anim.SetBool("Reload", false);
    }


    private void CrossColor()
    {
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, whatIsEnemy))
        {
            crosshair.color = rayHit.collider.CompareTag("Enemy") ? Color.red : Color.white;
        }
    }

}