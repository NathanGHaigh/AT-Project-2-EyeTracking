using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Eyeware.BeamEyeTracker.Unity;

public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool hovering;

    [SerializeField] private Item heldItem;

    private Image iconImage;

    private Color color;

    public AudioManager audioManager;

    [SerializeField] private TextMeshProUGUI slotItemName;

    private void Awake()
    {
        iconImage = transform.GetChild(0).GetComponent<Image>();
        color = this.GetComponent<Image>().color;
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Start()
    {
        slotItemName.text = "";
    }

    private void Update()
    {
        if (hovering)
        {
            this.GetComponent<Image>().color = Color.white;
            slotItemName.text = heldItem != null ? heldItem.objName : "";
        }
        else
        {
            this.GetComponent<Image>().color = color;
            slotItemName.text = "";
        }
    }

    public void SetHover(bool isHovering)
    {
        if (hovering == isHovering) return;

        hovering = isHovering;

        if(hovering)
        {
            audioManager?.HoveringItem();
        }
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
