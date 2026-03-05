using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool hovering;

    [SerializeField] private Item heldItem;

    private Image iconImage;

    private void Awake()
    {
        iconImage = transform.GetChild(0).GetComponent<Image>();
        
    }

    public Item GetItem()
    {
        return heldItem;
    }

    public void SetItem(Item item)
    {
        heldItem = item;

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (heldItem != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = heldItem.icon;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    public void ClearSlot()
    {
        heldItem = null;
        UpdateSlot();
    }

    public bool HasItem()
    {
        return heldItem != null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        Debug.Log(gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
