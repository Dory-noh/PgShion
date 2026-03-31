using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoxSpawn : MonoBehaviour
{
    public GameObject boxPrefab;
    public int boxCount = 3;
    public float range = 80f;
    public MiniMap miniMap;

    void Start()
    {
        SpawnBoxes();
        miniMap.InitBoxes();
    }

    float GetRandomExclude(float min, float max, float excludeMin, float excludeMax)
    {
        if (Random.value < 0.5f)
        {
            return Random.Range(min, excludeMin);
        }
        else
        {
            return Random.Range(excludeMax, max);
        }
    }

    void SpawnBoxes()
    {
        if (boxPrefab == null)
        {
            Debug.LogError("boxPrefabÀÌ ¿¬°á ¾È µÊ");
            return;
        }

        for (int i = 0; i < boxCount; i++)
        {
            float randomX =
                GetRandomExclude(-range, range, -20f, 20f);

            float randomZ =
                GetRandomExclude(-range, range, -20f, 20f);

            Vector3 spawnPos =
                new Vector3(randomX, 0.1f, randomZ);

            Quaternion randomRot =
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            Instantiate(boxPrefab, spawnPos, randomRot);
        }
    }
}