using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public static InventoryManager Instance;    // Tek bir InventoryManager olmasýný saðlamak için Singleton deseni

    // Envanter deðiþtiðinde (ekleme/silme) tetiklenecek bir olay (event)
    public delegate void OnChange();
    public static event OnChange OnInventoryChange;

    public GameObject inventoryPanel;   // Envanter paneli (UI kýsmý)
    public List<Slot> inventory = new List<Slot>();     // Oyuncunun envanterindeki slot listesi

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        SetInventoryPanelActive();
    }

    // Envantere item ekleme iþlemi
    public void AddItem(Item item)
    {
        // Stacklenemeyen item'ler direkt yeni bir slota eklenir
        if (item is not StackableItem)
        {
            AddNewItem(item);
            OnInventoryChange?.Invoke();
            return;
        }

        // Stacklenebilen item için mevcut dolmamýþ bir slot bul
        Slot existingSlot = inventory.Find(slot =>
            slot.item != null &&
            slot.item.itemID == item.itemID &&
            !slot.isFull);

        if (existingSlot != null)
        {
            // Ayný item varsa, mevcut slota ekle (stack artýr)
            existingSlot.IncreaseStackCount();
        }
        else
        {
            // Yoksa yeni bir slot oluþtur
            AddNewItem(item);
        }

        // Envanter deðiþtiðini bildir
        OnInventoryChange?.Invoke();
    }

    
    // Yeni bir item’i boþ slota ekle
    public void AddNewItem(Item item)
    {
        // Boþ slot bul
        Slot emptySlot = inventory.Find(slot => slot.item == null);

        if (emptySlot != null)
        {
            // Boþ slot bulunduysa ekle
            emptySlot.AddItem(item);
        }
        else
        {
            // Boþ slot yoksa uyarý ver
            Debug.LogWarning($"{item.name} eklenemedi: boþ slot yok!");
        }
    }

    
    // Envanterden item çýkarma iþlemi
    public void RemoveItem(Item item)
    {
        // Silinecek item’in bulunduðu slotu bul
        Slot slotToRemove = inventory.Find(slot =>
            slot.item != null &&
            slot.item.itemID == item.itemID);

        if (slotToRemove != null)
        {
            // Item’i sil
            slotToRemove.RemoveItem();
            OnInventoryChange?.Invoke(); // UI’yý güncelle
        }
        else
        {
            // Item bulunamadýysa uyarý ver
            Debug.LogWarning($"{item.name} envanterde bulunamadý!");
        }
    }

   
    // Envanter panelini açýp kapatmak için kullanýlan fonksiyon
    private void SetInventoryPanelActive()
    {
        // I tuþuna basýldýðýnda envanteri aç/kapat
        if (Input.GetKeyDown(KeyCode.I))
        {
            CanvasGroup canvasGroup = inventoryPanel.GetComponent<CanvasGroup>();

            if (canvasGroup.alpha == 1)
            {
                // Envanter açýksa kapat
                Time.timeScale = 1f; 
                Cursor.lockState = CursorLockMode.Locked; 
                Cursor.visible = false;

                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                // Envanter kapalýysa aç
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
    }
}
