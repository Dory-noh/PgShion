using System;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    static int gIdx = 0;
    // 프리팹 대신 데이터(아이콘, ID 포함)를 담은 ItemData를 사용
    public ItemData[] itemDataList;
    public GameObject currentDisplayItem;
    public int selectedItemIndex;
    public Transform ItemPos; // 상자 위 아이템 배치 위치

    void Start()
    {
        SelectItem();
    }

    void SelectItem()
    {
        if (itemDataList.Length == 0) return;

        // 1. 인덱스 순서대로 생성(중복 없음)
        selectedItemIndex = gIdx++;

        // 2. 위치 잡고 생성
        Vector3 displayPos = ItemPos.position;

        // 데이터 안에 있는 프리팹을 생성
        currentDisplayItem = Instantiate(itemDataList[selectedItemIndex].itemPrefab, displayPos, Quaternion.identity);

        // 상자 자식으로 설정
        currentDisplayItem.transform.SetParent(ItemPos);
        currentDisplayItem.transform.localPosition = Vector3.zero;

        currentDisplayItem.SetActive(false);
    }

    public void ShowItem()
    {
        Debug.Log("안녕");
        Debug.Log("아이템 null 여부" +this.currentDisplayItem);
        if (this.currentDisplayItem != null)
        {
            Debug.Log("켜져ㅑ라");
            this.currentDisplayItem.SetActive(true);
        }
    }

    // 아이템 획득 시 데이터 전체를 넘겨줌 (ID 체크)
    public ItemData GetItemData()
    {
        if (currentDisplayItem != null) Destroy(currentDisplayItem);
        return itemDataList[selectedItemIndex];
    }
}