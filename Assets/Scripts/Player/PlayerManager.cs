using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Health")]
    public float playerHealth = 100f;
    public Image bloodScreen;

    [Header("Boolean")]
    public bool isHaveKey;
    public bool isHaveCard;
    public bool isHaveGun;

    [Header("Gun Activation")]
    public GameObject hand;
    public GameObject ammoText;
    public Sprite crosshair;

    [Header("Ayarlar")]
    public RectTransform crosshairRect;
    public float restingSize;    
    public float movingSize;    
    public float speed;       
    private float currentSize;

    private void Update()
    {
        HealthManager();
        GunActivation();
        DynamicCrosshair();
    }

    private void HealthManager()
    {
        if (playerHealth >= 100) { playerHealth = 100; }
        if (playerHealth <= 0) { playerHealth = 0; }

        playerHealth = Mathf.Clamp(playerHealth, 0, 100);

        float alpha;

        if (playerHealth > 30)
        {
            float t = Mathf.InverseLerp(100f, 30f, playerHealth);
            alpha = Mathf.Lerp(0.2f, 1f, t);
        }
        else
        {
            alpha = 1f;
        }

        Color c = bloodScreen.color;
        c.a = alpha;
        bloodScreen.color = c;

        if(playerHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void GunActivation()
    {
        if (isHaveGun)
        {
            hand.SetActive(true);
            ammoText.SetActive(true);
            GameObject.FindWithTag("Cursor").GetComponent<Image>().sprite = crosshair;
            GameObject.FindWithTag("Cursor").GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    private void DynamicCrosshair()
    {
        if (isHaveGun)
        {
            bool isMoving = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
            float targetSize = isMoving ? movingSize : restingSize;

            currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * speed);
            crosshairRect.sizeDelta = new Vector2(currentSize, currentSize);
        }
    }
}
