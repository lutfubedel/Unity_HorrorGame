using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Interaction : MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "Kartý Al";
    }

    public void Interact()
    {
        GameObject.FindAnyObjectByType<PlayerManager>().isHaveCard = true;
        Debug.Log("Kart Alýndý");
        Destroy(this.gameObject);
    }
}
