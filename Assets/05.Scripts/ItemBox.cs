using UnityEngine;

public class ItemBox : MonoBehaviour
{
    // 프리팹 대신 데이터(아이콘, ID 포함)를 담은 ItemData를 사용합니다.
    public ItemData[] itemDataList;
    private GameObject currentDisplayItem;
    public int selectedItemIndex;
    public Transform ItemPos; // 상자 위 아이템 배치 위치

    void Start()
    {
        SelectItem();
    }

    void SelectItem()
    {
        if (itemDataList.Length == 0) return;

        // 1. 랜덤 선택
        selectedItemIndex = Random.Range(0, itemDataList.Length);

        // 2. 위치 잡고 생성
        Vector3 displayPos = ItemPos.position;

        // 데이터 안에 있는 프리팹을 생성합니다.
        currentDisplayItem = Instantiate(itemDataList[selectedItemIndex].itemPrefab, displayPos, Quaternion.identity);

        currentDisplayItem.SetActive(true);

        // 상자 자식으로 설정
        currentDisplayItem.transform.SetParent(this.transform);
    }

    // 아이템 획득 시 데이터 전체를 넘겨줍니다 (그래야 ID 체크 가능)
    public ItemData GetItemData()
    {
        if (currentDisplayItem != null) Destroy(currentDisplayItem);
        return itemDataList[selectedItemIndex];
    }
}