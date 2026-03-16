using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Eyeware.BeamEyeTracker.Unity;
using System.Data.Common;

public class Inventory : BeamEyeTrackerMonoBehaviour
{
    public GameObject player;

    public GameObject InventoryUI;

    public PlayerInput playerInput;

    public Camera playerCamera;

    public Camera forwardCamera;

    public Image dragImage;

    public Image equippedItemImage;

    public AudioManager audioManager;

    [SerializeField]
    BeamEyeTrackerInputDevice eyeTrackerInputDevice;

    public GameObject inventorySlotParent;

    private InvenSlot dragSlot = null;
    private bool isDragging = false;

    public List<InvenSlot> inventorySlots = new List<InvenSlot>();

    [SerializeField]
    Vector2 gazeScreenOffset = new Vector2(0f, -20f);

    [SerializeField]
    RectTransform gazeIndicator;
    private Canvas parentCanvas;

    [SerializeField]
    private Item heldItem;

    private void OnEnable()
    {
        playerInput.actions["InventorySelect"].started += HandleCurrentProcess;
        playerInput.actions["InventoryUse"].started += UseItemSelected;
    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        if(audioManager == null)
            audioManager = FindAnyObjectByType<AudioManager>();
        if (inventorySlotParent != null)
            inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<InvenSlot>());

        eyeTrackerInputDevice = betInputDevice;

        parentCanvas = inventorySlotParent != null ? inventorySlotParent.GetComponentInParent<Canvas>() : null;

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (gazeIndicator != null)
            gazeIndicator.gameObject.SetActive(true);
    }

    private void Update()
    {
        GetEyeTrackPos();
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

    void HandleCurrentProcess(InputAction.CallbackContext context)
    {
        if (dragSlot == null)
        {
            StartDrag();
        }
        else
        {
            EndDrag();
        }
    }

    private void StartDrag()
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

    private void EndDrag()
    {
        InvenSlot hovered = GetHoveredSlot();
        if (hovered != null)
        {
            if (dragSlot != null)
            {
                HandleDrop(dragSlot, hovered);

                dragImage.enabled = false;

                dragSlot = null;
                isDragging = false;
            }
        }
        else if(hovered == null)
        {
            HandleDropItem();
        }
    }
    private void UseItemSelected(InputAction.CallbackContext context)
    {
        if(dragSlot != null)
        {
            if(dragSlot.GetItem() is ItemKeyCard)
            {
                equippedItemImage.sprite = dragSlot.GetItem().icon;
                equippedItemImage.enabled = true;
                player.GetComponentInChildren<PlayerController>().heldItem = dragSlot.GetItem();
                dragSlot = null;
                isDragging = false;
                dragImage.enabled = false;
                player.GetComponentInChildren<PlayerController>().inventoryActive = false;    
            }
            else
            {
                Debug.Log("No Other CUrrent Item Functionality");
            }
        }
    }


    private void HandleDropItem()
    {
        Debug.Log("Dropping Item");

        Item item = dragSlot.GetItem();
        GameObject prefab = item.prefab;
        Debug.Log(prefab.name);

        GameObject dropped = Instantiate(prefab, forwardCamera.transform.position + forwardCamera.transform.forward, Quaternion.identity);


        ItemDrop itemDrop = dropped.GetComponentInChildren<ItemDrop>();
        itemDrop.item = item;
        dragSlot.ClearSlot();
        dragSlot = null;
        isDragging= false;
        dragImage.enabled = false;
        


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
        if (from == to) return;

        if (to.HasItem())
        {
            Item tempItem = to.GetItem();
            to.SetItem(from.GetItem());
            from.SetItem(tempItem);
            return;
        }

        to.SetItem(from.GetItem());
        from.ClearSlot();
    }

    private void UpdateDragItemPos()
    {
        if (isDragging)
        {
            Vector2 currentGazePos = eyeTrackerInputDevice.viewportGazePosition.ReadValue();
            currentGazePos.x = Mathf.Clamp01(currentGazePos.x);
            currentGazePos.y = Mathf.Clamp01(currentGazePos.y);

            Vector2 screenPoint = (playerCamera.ViewportToScreenPoint(currentGazePos));
            screenPoint += gazeScreenOffset;

            dragImage.transform.position = screenPoint;

            UpdateGazeIndicator(screenPoint);
        }
    }

    private void GetEyeTrackPos()
    {
        Vector2 currentGazePos = eyeTrackerInputDevice.viewportGazePosition.ReadValue();
        currentGazePos.x = Mathf.Clamp01(currentGazePos.x);
        currentGazePos.y = Mathf.Clamp01(currentGazePos.y);

        Vector2 screenPoint = (playerCamera.ViewportToScreenPoint(currentGazePos));
        screenPoint += gazeScreenOffset;
    
        //Debug.Log($"Current gaze position: {screenPoint}");

        UpdateGazeIndicator(screenPoint);

        foreach (InvenSlot slot in inventorySlots)
        {
            if (slot == null) continue;
            RectTransform rt = slot.GetComponent<RectTransform>();

            bool contains = RectTransformUtility.RectangleContainsScreenPoint(
                rt,
                screenPoint
            );

            slot.SetHover(contains);

        }

    }

    private void UpdateGazeIndicator(Vector2 screenPoint)
    {
        if (gazeIndicator == null)
            return;

        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            RectTransform canvasRect = parentCanvas.transform as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPoint,
                parentCanvas.worldCamera,
                out Vector2 localPoint
            );
            gazeIndicator.anchoredPosition = localPoint;
        }
        else
        {
            gazeIndicator.position = screenPoint;
        }
    }
}
