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
    }



}
