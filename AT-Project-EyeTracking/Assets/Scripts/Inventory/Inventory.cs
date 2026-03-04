using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public PlayerInput playerInput;

    public Item keycard;

    public GameObject inventorySlotParent;

    private List<InvenSlot> inventorySlots = new List<InvenSlot>();

    private void OnEnable()
    {
        playerInput.actions["DebugItems"].started += SpawnItem;
    }

    private void OnDisable()
    {
        playerInput.actions["DebugItems"].canceled -= SpawnItem;
    }

    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<InvenSlot>());
    }

    public void AddItem(Item itemToAdd)
    {
        foreach (InvenSlot slot in inventorySlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {


            }
        }
        foreach(InvenSlot slot in inventorySlots)
        {
            if(!slot.HasItem())
            {
                slot.SetItem(itemToAdd);
            }
        }

    }
    private void SpawnItem(InputAction.CallbackContext context)
    {
        AddItem(keycard);
    }



}
