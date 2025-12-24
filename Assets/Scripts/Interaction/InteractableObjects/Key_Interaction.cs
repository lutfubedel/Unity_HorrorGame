using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Interaction : MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "Anahtarý Al";
    }

    public void Interact()
    {
        GameObject.FindAnyObjectByType<PlayerManager>().isHaveKey = true;
        Debug.Log("Anahtar Alýndý");
        Destroy(this.gameObject);
    }
}
