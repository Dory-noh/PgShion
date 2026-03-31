using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public RectTransform mapRect;
    public RectTransform playerIcon;
    public List<Transform> itemBoxes;
    public GameObject iconPrefab;
    private List<RectTransform> icons = new List<RectTransform>();

    public float mapSizeWorld = 300f; // 150 * 2

    public void InitBoxes()
    {
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("ItemBox");

        foreach (var obj in boxes)
        {
            itemBoxes.Add(obj.transform);

            GameObject icon = Instantiate(iconPrefab, mapRect);
            icons.Add(icon.GetComponent<RectTransform>());
        }
    }

    void Update()
    {
        // 위치
        playerIcon.anchoredPosition = WorldToMapPosition(player.position);

        // 방향
        float angle = player.eulerAngles.y;
        playerIcon.localRotation = Quaternion.Euler(0, 0, -angle+180);

        // 아이템
        for (int i = 0; i < itemBoxes.Count; i++)
        {
            if (itemBoxes[i] == null)
            {
                icons[i].gameObject.SetActive(false);
                continue;
            }

            icons[i].anchoredPosition = WorldToMapPosition(itemBoxes[i].position);
        }
    }

    Vector2 WorldToMapPosition(Vector3 worldPos)
    {
        float normalizedX = (worldPos.x + 150f) / mapSizeWorld;
        float normalizedY = (worldPos.z + 150f) / mapSizeWorld;

        Vector2 mapSize = mapRect.sizeDelta;

        float uiX = (normalizedX - 0.5f) * mapSize.x;
        float uiY = (normalizedY - 0.5f) * mapSize.y;

        return new Vector2(uiX, uiY);
    }
}