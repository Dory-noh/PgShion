using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image iconImage;
    public int slotIndex;

    private ItemData storedItem;
    private SnailItemHandler player;

    private float pressTime;
    private bool isHolding;
    private bool isDragging;

    private static ItemSlot originSlot;
    private static Image dragIcon;

    void Awake()
    {
        player = FindFirstObjectByType<SnailItemHandler>();
        ClearSlot();
    }

    void Update()
    {
        if (isHolding)
        {
            pressTime += Time.deltaTime;

            if (pressTime >= 0.5f && !isDragging && storedItem != null)
            {
                StartDrag();
            }
        }
    }

    void StartDrag()
    {
        isDragging = true;
        originSlot = this;

        dragIcon = new GameObject("DragIcon").AddComponent<Image>();
        dragIcon.transform.SetParent(transform.root);
        dragIcon.raycastTarget = false;
        dragIcon.sprite = iconImage.sprite;
        dragIcon.rectTransform.sizeDelta = iconImage.rectTransform.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && dragIcon != null)
        {
            dragIcon.transform.position = eventData.position;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        pressTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            GameObject targetObj = eventData.pointerCurrentRaycast.gameObject;

            if (targetObj != null)
            {
                ItemSlot targetSlot = targetObj.GetComponentInParent<ItemSlot>();

                if (targetSlot != null && originSlot != null)
                {
                    Swap(originSlot, targetSlot);
                }
            }

            EndDrag();
        }

        isHolding = false;
        pressTime = 0f;
    }

    void EndDrag()
    {
        isDragging = false;

        if (dragIcon != null)
            Destroy(dragIcon.gameObject);

        dragIcon = null;
        originSlot = null;
    }

    void Swap(ItemSlot a, ItemSlot b)
    {
        ItemData temp = a.storedItem;
        a.SetItem(b.storedItem);
        b.SetItem(temp);

        UpdateEquip();
    }

    public void SetItem(ItemData data)
    {
        storedItem = data;

        if (data != null)
        {
            iconImage.sprite = data.itemIcon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    void UpdateEquip()
    {
        ItemSlot[] allSlots = FindObjectsByType<ItemSlot>(FindObjectsSortMode.None);

        foreach (var slot in allSlots)
        {
            if (slot.slotIndex == 0)
            {
                player.EquipFromSlot(slot.GetItem());
                break;
            }
        }
    }

    public void ClearSlot()
    {
        SetItem(null);
    }

    public ItemData GetItem()
    {
        return storedItem;
    }

    public bool HasItem()
    {
        return storedItem != null;
    }
}