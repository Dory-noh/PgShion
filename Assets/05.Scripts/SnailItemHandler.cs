using UnityEngine;

public class SnailItemHandler : MonoBehaviour
{
    public ItemInventoryUI inventoryUI;
    public Transform itemHolder;

    private GameObject currentActiveItem;
    private ItemData currentItem;

    public GameObject shakeBtn;
    public GameObject getItemBtn;

    private GameObject selectedObj;
    public GameObject[] chatDoorMans;
    private int index = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            selectedObj = other.gameObject;
            shakeBtn.SetActive(true);
        }

        if (other.CompareTag("door"))
        {
            foreach (var obj in chatDoorMans)
                obj.SetActive(false);

            chatDoorMans[index].SetActive(true);

            // ¥Ÿ¿Ω ¿Œµ¶Ω∫
            index = (index + 1) % chatDoorMans.Length;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ItemBox"))
        {
            selectedObj = null;
            shakeBtn.SetActive(false);
            getItemBtn.SetActive(false);
        }

        if (other.CompareTag("door"))
        {
            foreach (var obj in chatDoorMans)
                obj.SetActive(false);
        }
    }

    public void GetItem()
    {
        if (selectedObj == null) return;

        ItemBox box = selectedObj.GetComponent<ItemBox>();
        if (box == null) return;

        ItemData data = box.GetItemData();

        //EquipItem(data);
        inventoryUI.TryAddItemToSlot(data);

        Destroy(selectedObj);
    }

    public void EquipItem(ItemData data)
    {
        if (data == null) return;

        currentItem = data;

        if (currentActiveItem != null)
            Destroy(currentActiveItem);

        currentActiveItem = Instantiate(
            data.itemPrefab,
            itemHolder.position,
            itemHolder.rotation
        );

        currentActiveItem.transform.SetParent(itemHolder);
        currentActiveItem.SetActive(true);

        shakeBtn.SetActive(false);
        getItemBtn.SetActive(false);
    }

    public void UnequipItem()
    {
        currentItem = null;

        if (currentActiveItem != null)
        {
            Destroy(currentActiveItem);
            currentActiveItem = null;
        }
    }

    public void EquipFromSlot(ItemData data)
    {
        if (data != null)
            EquipItem(data);
        else
            UnequipItem();
    }
}