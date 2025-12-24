using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit_Interaction : MonoBehaviour, IInteractable
{
    public float healthPlus;

    public string GetInteractionText()
    {
        return "Saðlýk Kitini Al";
    }

    public void Interact()
    {
        GameObject.FindAnyObjectByType<PlayerManager>().playerHealth += healthPlus;
        Debug.Log("Saðlýk Kiti alýndý");
        Destroy(this.gameObject);
    }
}
