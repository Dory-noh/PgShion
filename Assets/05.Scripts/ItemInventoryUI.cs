using UnityEngine;
using System.Collections.Generic;

public class ItemInventoryUI : MonoBehaviour
{
    public List<ItemSlot> slots; // 인스펙터에서 슬롯들을 드래그해서 넣으세요

    public void TryAddItemToSlot(ItemData data)
    {
        // 1. 중복 체크 (이미 슬롯에 같은 ID가 있다면 종료)
        foreach (ItemSlot slot in slots)
        {
            if (slot.isFull && slot.GetStoredItemID() == data.itemID)
            {
                Debug.Log("이미 인벤토리에 있는 아이템입니다.");
                return;
            }
        }

        // 2. 중복이 아니면 비어있는 첫 번째 슬롯에 추가
        foreach (ItemSlot slot in slots)
        {
            if (!slot.isFull)
            {
                slot.AddItem(data);
                return;
            }
        }
    }
}