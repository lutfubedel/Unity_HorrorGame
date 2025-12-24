using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slot
{
    public Item item;
    public int itemCount;
    public bool isFull;

    public bool IsEmpty => item == null;

    private int MaxStackCount
    {
        get
        {
            if (item is StackableItem sItem)
                return sItem.maxStackCount;
            return 1;
        }
    }

    public bool CanAddItem(Item itemToAdd)
    {
        if (IsEmpty) return true;
        if (item.itemID != itemToAdd.itemID) return false;
        if (item is not StackableItem) return false;
        return itemCount < MaxStackCount;
    }

    public void AddItem(Item itemToAdd)
    {
        if (!CanAddItem(itemToAdd))
        {
            Debug.LogWarning($"Slot: {itemToAdd.name} eklenemedi — dolu veya farklý item türü.");
            return;
        }

        if (IsEmpty)
            item = itemToAdd;

        itemCount++;
        CheckIsFull();
    }

    public void IncreaseStackCount()
    {
        if (item is StackableItem && itemCount < MaxStackCount)
        {
            itemCount++;
            CheckIsFull();
        }
    }

    private void CheckIsFull()
    {
        if (item is StackableItem)
            isFull = itemCount >= MaxStackCount;
        else
            isFull = item != null;
    }

    public void RemoveItem()
    {
        if (itemCount <= 0) return;

        itemCount--;
        CheckIsFull();

        if (itemCount <= 0)
        {
            itemCount = 0;
            isFull = false;
            item = null;
        }
    }
}
