using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    private Button button;
    private Sprite emptySprite;

    public Image previewImageHolder;
    public TMP_Text itemCountText;
    public Slot currentSlot;
    public bool isEmpty;


    private void Awake()
    {
        isEmpty = false;
        emptySprite = previewImageHolder.sprite;
        button = GetComponent<Button>();
    }

    public void AttachItem(Slot slot)
    {
        currentSlot = slot;
        previewImageHolder.sprite = currentSlot.item.previewImage;
        itemCountText.text = currentSlot.itemCount.ToString();
        isEmpty = false;
    }

    public void ClearSlot()
    {
        currentSlot = null;
        previewImageHolder.sprite = emptySprite;
        itemCountText.text = "";
        isEmpty = true;
    }

    public bool GetIsEmpty()
    {
        return isEmpty;
    }




}
