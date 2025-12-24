using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill_Interaction : MonoBehaviour, IInteractable
{
    public float healthPlus;

    string IInteractable.GetInteractionText()
    {
        return "Haplarý Al";
    }

    void IInteractable.Interact()
    {
        GameObject.FindWithTag("GlobalVolume").GetComponent<SanityController>().currentSanity += healthPlus;
        Debug.Log("Hap alýndý");
        Destroy(this.gameObject);
    }
}
