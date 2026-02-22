using UnityEngine;

public class BoxSpawn : MonoBehaviour
{
    public GameObject boxPrefab; // 생성할 box 프리팹
    public int snailCount = 10;    // 생성 개수
    public float range = 125f;      // -125 ~ 125 범위

    void Start()
    {
        SpawnBoxes();
    }

    void SpawnBoxes()
    {
        if (boxPrefab == null)
        {
            Debug.LogError("Snail Prefab이 연결 안 됨");
            return;
        }

        for (int i = 0; i < snailCount; i++)
        {
            // 1. 랜덤 위치 계산 (X, Z축 -125 ~ 125)
            float randomX = Random.Range(-range, range);
            float randomZ = Random.Range(-range, range);

            Vector3 spawnPos = new Vector3(randomX, 0.1f, randomZ);

            // 2. 랜덤 회전 (미리 아무 방향이나 보고 있게)
            Quaternion randomRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // 3. 생성
            Instantiate(boxPrefab, spawnPos, randomRot);
        }
    }
}