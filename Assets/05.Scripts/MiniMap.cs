using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public RectTransform mapRect;
    public RectTransform playerIcon;

    public List<Transform> itemBoxes = new List<Transform>();
    public GameObject iconPrefab;
    private List<RectTransform> icons = new List<RectTransform>();

    public float mapSizeWorld = 300f; // 전체 월드 크기 (-150 ~ 150)

    void Start()
    {
        InitBoxes();
    }

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
        if (player == null) return;

        // ?? 플레이어 위치
        playerIcon.anchoredPosition = WorldToMapPosition(player.position);

        // ?? 플레이어 방향
        float angle = player.eulerAngles.y;
        playerIcon.localRotation = Quaternion.Euler(0, 0, -angle + 180f);

        // ?? 아이템 위치
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
        // ?? -150 ~ 150 → 0 ~ 1 정규화
        float normalizedX = (worldPos.x + mapSizeWorld * 0.5f) / mapSizeWorld;
        float normalizedY = (worldPos.z + mapSizeWorld * 0.5f) / mapSizeWorld;

        Vector2 mapSize = mapRect.sizeDelta;

        // ?? UI 좌표 변환 (중앙 기준)
        float uiX = (normalizedX - 0.5f) * mapSize.x;

        // Z → Y 변환 시 반전
        float uiY = -(normalizedY - 0.5f) * mapSize.y;

        return new Vector2(uiX, uiY);
    }
}