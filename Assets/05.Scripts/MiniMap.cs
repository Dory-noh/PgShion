using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public RectTransform mapRect;
    public RectTransform playerIcon;

    public float mapSizeWorld = 300f; // 150 * 2

    void Update()
    {
        Vector3 pos = player.position;

        // 1. 정규화 (0~1)
        float normalizedX = (pos.x + 150f) / mapSizeWorld;
        float normalizedY = (pos.z + 150f) / mapSizeWorld;

        // 2. UI 좌표 변환
        Vector2 mapSize = mapRect.sizeDelta;

        float uiX = (normalizedX - 0.5f) * mapSize.x;
        float uiY = (normalizedY - 0.5f) * mapSize.y;

        playerIcon.anchoredPosition = new Vector2(uiX, uiY);
    }
}