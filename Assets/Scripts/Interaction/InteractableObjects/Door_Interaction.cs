using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Interaction : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen;

    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool isCorridor;

    private float openAngle = 90f;
    private float closeAngle = 0f;

    PlayerManager playerManager;

    private void Start()
    {
        playerManager = GameObject.FindFirstObjectByType<PlayerManager>();
    }
    private void Update()
    {
        float targetY = isOpen ? openAngle : closeAngle;

        Quaternion targetRotation = Quaternion.Euler(0, targetY, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    public string GetInteractionText()
    {
        if (isCorridor)
        {
            if (playerManager.isHaveCard)
            {
                return isOpen ? "Kapýyý Kapat" : "Kapýyý Aç";
            }
            else
            {
                return "Kilitli (Kart Gerekli)";
            }
        }
        else
        {
            if (playerManager.isHaveKey)
            {
                return isOpen ? "Kapýyý Kapat" : "Kapýyý Aç";
            }
            else
            {
                return "Kilitli (Anahtar Gerekli)";
            }
        }

    }

    public void Interact()
    {
        if (isCorridor)
        {
            if (playerManager.isHaveCard)
            {
                isOpen = !isOpen;
                Debug.Log(isOpen ? "Kapý Açýlýyor" : "Kapý Kapanýyor");
            }
        }
        else
        {
            if (playerManager.isHaveKey)
            {
                isOpen = !isOpen;
                Debug.Log(isOpen ? "Kapý Açýlýyor" : "Kapý Kapanýyor");
            }
        }

    }
}