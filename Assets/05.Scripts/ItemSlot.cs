using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image iconImage;      // 아이콘 표시용 Image 컴포넌트
    public bool isFull = false;  // 슬롯이 차 있는지 확인
    private ItemData storedItem;
    private SnailItemHandler player;

    void Awake()
    {
        player = Object.FindFirstObjectByType<SnailItemHandler>();
        ClearSlot();
    }

    public void AddItem(ItemData data)
    {
        storedItem = data;
        iconImage.sprite = data.itemIcon;
        iconImage.enabled = true;
        isFull = true;
    }

    public string GetStoredItemID() => storedItem != null ? storedItem.itemID : "";

    public void OnSlotClick() // 버튼의 OnClick에 연결하세요
    {
        if (isFull && storedItem != null)
        {
            player.EquipItem(storedItem); // 클릭하면 다시 장착
        }
    }

    public void ClearSlot()
    {
        storedItem = null;
        if (iconImage != null) iconImage.enabled = false;
        isFull = false;
    }
}