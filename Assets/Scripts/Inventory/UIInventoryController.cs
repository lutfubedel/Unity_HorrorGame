using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    public List<SlotUI> uiInventory = new List<SlotUI>();

    private void OnEnable()
    {
        InventoryManager.OnInventoryChange += UpdateInventory;
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryChange -= UpdateInventory;
    }



    private void ClearAll()
    {
        uiInventory.ForEach(slot=>slot.ClearSlot());
    }

    private SlotUI GetSlot()
    {
        foreach(SlotUI slot in uiInventory)
        {
            if (slot.GetIsEmpty())
            {
                return slot;
            }
        }
        return null;
    }


    private void UpdateInventory() 
    { 
        ClearAll();
        foreach(Slot slot in InventoryManager.Instance.inventory)
        {
            if(slot.item != null)
            {
                GetSlot()?.AttachItem(slot);
            }
        }
    }

}
