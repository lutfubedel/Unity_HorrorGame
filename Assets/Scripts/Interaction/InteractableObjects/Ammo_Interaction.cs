using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Interaction : MonoBehaviour, IInteractable
{
    public int ammoCount;
    public string GetInteractionText()
    {
        return $"Cephane Al ({ammoCount})";
    }

    public void Interact()
    {
        GameObject.FindWithTag("Gun").GetComponent<GunSystem>().totalAmmo += ammoCount;
        Debug.Log("Mermi Alýndý");
        Destroy(this.gameObject);
    }
}
