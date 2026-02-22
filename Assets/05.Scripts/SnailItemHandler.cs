using UnityEngine;

public class SnailItemHandler : MonoBehaviour
{
    public ItemInventoryUI inventoryUI; // 인벤토리 관리자 연결
    public Transform itemHolder;        // 아이템이 붙을 달팽이 등 위치
    private GameObject currentActiveItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            ItemBox box = other.GetComponent<ItemBox>();
            if (box != null)
            {
                // 상자에서 데이터(ID, 아이콘, 프리팹 세트)를 받아옴
                ItemData data = box.GetItemData();

                // 1. 즉시 적용 (등 위에 나타남)
                EquipItem(data);

                // 2. 슬롯에 추가 (중복 체크는 내부에서 수행)
                inventoryUI.TryAddItemToSlot(data);

                Destroy(other.gameObject);
            }
        }
    }

    public void EquipItem(ItemData data)
    {
        // 이전 아이템 삭제
        if (currentActiveItem != null) Destroy(currentActiveItem);

        // 새 아이템 생성
        currentActiveItem = Instantiate(data.itemPrefab, itemHolder.position, itemHolder.rotation);
        currentActiveItem.transform.SetParent(itemHolder);
        currentActiveItem.SetActive(true);
    }
}