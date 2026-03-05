using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public PlayerInput playerInput;

    public Item keycard;

    public Item eyeDrops;

    public Image dragImage;

    public GameObject inventorySlotParent;

    private InvenSlot dragSlot = null;
    private bool isDragging = false;

    private List<InvenSlot> inventorySlots = new List<InvenSlot>();

    private void OnEnable()
    {
        playerInput.actions["DebugItems"].started += SpawnItem;
        playerInput.actions["InventorySelect"].started += StartDrag; 
        playerInput.actions["InventoryPlace"].canceled += EndDrag;
    }

    private void OnDisable()
    {
        playerInput.actions["DebugItems"].canceled -= SpawnItem;

    }

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<InvenSlot>());
    }

    private void Update()
    { 
            UpdateDragItemPos();
    }

    public void AddItem(Item itemToAdd)
    {
        var emptySlot = inventorySlots.Find(slot => !slot.HasItem());
        if (emptySlot != null)
        {
            emptySlot.SetItem(itemToAdd);
        }
        else
        {
            Debug.Log("No empty inventory slots!");
        }

    }
    private void SpawnItem(InputAction.CallbackContext context)
    {
        AddItem(keycard);
        AddItem(eyeDrops);
    }

    private void StartDrag(InputAction.CallbackContext context)
    {
        Debug.Log("Started Drag Functiom");
        InvenSlot hovered = GetHoveredSlot();
        if (hovered != null && hovered.HasItem())
        {
            Debug.Log("Dragging");
            dragSlot = hovered;
            isDragging = true;
            dragImage.sprite = hovered.GetItem().icon;
            dragImage.color = new Color(1, 1, 1, 0.5f);
            dragImage.enabled = true;

        }

    }

    private void EndDrag(InputAction.CallbackContext context)
    {
        InvenSlot hovered = GetHoveredSlot();
        if(hovered != null)
        {
            HandleDrop(dragSlot, hovered);

            dragImage.enabled = false;

            dragSlot = null;
            isDragging= false;

        }

    }

    private InvenSlot GetHoveredSlot()
    {
        foreach (InvenSlot s in inventorySlots)
        {
            if (s.hovering)
                return s;
        }
        return null;
    }

    private void HandleDrop(InvenSlot from, InvenSlot to)
    {
        if(from == to) return;

        if(to.HasItem())
        {
            Item tempItem= to.GetItem();
            to.SetItem(from.GetItem());
            from.SetItem(tempItem);
            return;
        }

        to.SetItem(from.GetItem());
        from.ClearSlot();
    }

    private void UpdateDragItemPos()
    {
        if(isDragging)
        {
            dragImage.transform.position = Input.mousePosition;

        }
    }
}
