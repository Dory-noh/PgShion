using UnityEngine;
using System.Collections.Generic;

public class ItemInventoryUI : MonoBehaviour
{
    public List<ItemSlot> slots;

    public void TryAddItemToSlot(ItemData data)
    {
        foreach (ItemSlot slot in slots)
        {
            if (slot.HasItem() && slot.GetItem().itemID == data.itemID)
            {
                Debug.Log("이미 인벤토리에 있는 아이템입니다.");
                return;
            }
        }

        foreach (ItemSlot slot in slots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(data);
                return;
            }
        }
    }
}