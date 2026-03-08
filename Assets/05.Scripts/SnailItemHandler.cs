using UnityEngine;

public class SnailItemHandler : MonoBehaviour
{
    public ItemInventoryUI inventoryUI; // 인벤토리 관리자 연결
    public Transform itemHolder;        // 아이템이 붙을 달팽이 등 위치
    private GameObject currentActiveItem;
    public GameObject shakeBtn;
    public GameObject getItemBtn;
    private GameObject selectedObj;
    public GameObject chatDoorMan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            selectedObj = other.gameObject;
            shakeBtn.SetActive(true);
        }

        if (other.CompareTag("door"))
        {
            chatDoorMan.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            if (shakeBtn.activeSelf == true)
            {
                selectedObj = null;
                getItemBtn.SetActive(false);
                shakeBtn.SetActive(false);
            }
        }
        
        if (other.CompareTag("door"))
        {
            chatDoorMan.SetActive(false);
        }
    }
    
    public void ShowItem()
    {
        ItemBox box = selectedObj.GetComponent<ItemBox>();
        if(box!=null)
            box.ShowItem();
    }


    public void GetItem()
    {   
        ItemBox box = selectedObj.GetComponent<ItemBox>();
        if (box != null)
        {
            // 상자에서 데이터(ID, 아이콘, 프리팹 세트)를 받아옴
            ItemData data = box.GetItemData();

            // 1. 즉시 적용 (등 위에 나타남)
            EquipItem(data);

            // 2. 슬롯에 추가 (중복 체크는 내부에서 수행)
            inventoryUI.TryAddItemToSlot(data);

            Destroy(selectedObj);
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
        shakeBtn.SetActive(false);
        getItemBtn.SetActive(false);
    }
}