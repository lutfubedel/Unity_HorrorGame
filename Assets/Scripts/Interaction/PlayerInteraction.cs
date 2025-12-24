using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;     // Oyuncunun etkileþimde bulunabileceði maksimum mesafe
    public GameObject interactionText;        // Etkileþim metninin gösterileceði UI öðesi (TextMeshPro)


    IInteractable currentInteractable;
    Camera playerCamera;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        CheckForInteractable();

        // Eðer bir etkileþilebilir nesneye bakýlýyorsa ve E tuþuna basýlmýþsa
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact(); // Nesnenin kendi etkileþim davranýþýný çalýþtýr
        }
    }

    // Oyuncunun baktýðý yönde etkileþilebilir bir nesne olup olmadýðýný kontrol eder
    void CheckForInteractable()
    {
        // Kamera merkezinden ileriye doðru bir ray (ýþýn) gönderilir
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Raycast ile belirlenen mesafe içinde çarpýlan nesne varsa
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // Çarpýlan nesnede IInteractable arayüzüne sahip bir bileþen var mý kontrol edilir
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            // Eðer varsa bu nesneyle etkileþime geçilebilir
            if (interactable != null)
            {
                // UI metnini nesnenin verdiði etkileþim yazýsý ile güncelle
                interactionText.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = interactable.GetInteractionText();

                // UI yazýsýný aktif et
                interactionText.SetActive(true); 

                // Bu nesneyi mevcut etkileþilebilir olarak ata
                currentInteractable = interactable;
                return; 
            }
        }

        // Eðer hiçbir etkileþilebilir nesneye bakýlmýyorsa:
        interactionText.SetActive(false);
        currentInteractable = null;       
    }
}
