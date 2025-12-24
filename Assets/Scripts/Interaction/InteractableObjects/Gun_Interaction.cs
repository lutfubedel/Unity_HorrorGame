using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Interaction : MonoBehaviour, IInteractable
{
    public string GetInteractionText()
    {
        return "Silahý Al";
    }

    public void Interact()
    {
        // 1. Adým: Önce oyuncuyu bulmaya çalýþalým
        PlayerManager playerObj = GameObject.FindAnyObjectByType<PlayerManager>();

        if (playerObj == null)
        {
            Debug.LogError("HATA: Sahnede 'Player' tagine sahip bir obje bulunamadý!");
            return; // Hata varsa iþlemi durdur
        }

        // 3. Adým: Her þey tamamsa iþlemi yap
        playerObj.isHaveGun = true;
        Debug.Log("Silah Alýndý");
        Destroy(this.gameObject);
    }
}
