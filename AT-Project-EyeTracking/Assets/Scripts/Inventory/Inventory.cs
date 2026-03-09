using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Eyeware.BeamEyeTracker.Unity;
using System.Data.Common;

public class Inventory : BeamEyeTrackerMonoBehaviour
{
    public PlayerInput playerInput;

    public Camera playerCamera;

    public Item keycard;

    public Item eyeDrops;

    public Image dragImage;

    [SerializeField]
    BeamEyeTrackerInputDevice eyeTrackerInputDevice;

    public GameObject inventorySlotParent;

    private InvenSlot dragSlot = null;
    private bool isDragging = false;

    private List<InvenSlot> inventorySlots = new List<InvenSlot>();

    [SerializeField]
    Vector2 gazeScreenOffset = new Vector2(0f, -20f);

    [SerializeField]
    RectTransform gazeIndicator;
    private Canvas parentCanvas;

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
        if (hovered != null)
        {
            HandleDrop(dragSlot, hovered);

            dragImage.enabled = false;

            dragSlot = null;
            isDragging = false;

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
    
        Debug.Log($"Current gaze position: {screenPoint}");

        UpdateGazeIndicator(screenPoint);

        foreach (InvenSlot slot in inventorySlots)
        {
            if (slot == null) continue;
            RectTransform rt = slot.GetComponent<RectTransform>();

            bool contains = RectTransformUtility.RectangleContainsScreenPoint(
                rt,
                screenPoint
            );

            if (slot.hovering != contains)
            {
                slot.hovering = contains;
            }

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
