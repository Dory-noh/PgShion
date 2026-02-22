using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Data")]
public class ItemData : ScriptableObject
{
    public string itemID;        // "Item_A" 처럼 고유한 이름을 쓰세요 (중복 체크용)
    public string itemName;      // 표시될 이름
    public Sprite itemIcon;      // UI에 보일 이미지
    public GameObject itemPrefab; // 달팽이 등에 생성될 실제 모델
}