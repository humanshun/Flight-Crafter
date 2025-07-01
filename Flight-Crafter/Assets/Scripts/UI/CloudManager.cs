using UnityEngine;
using System.Collections.Generic;

public class CloudManager : MonoBehaviour
{
    public Transform playerPosition;
    public GameObject[] cloudPrefabs;

    [Header("表示設定")]
    public int cloudCount = 50;
    public float spawnRadius = 500f;
    public float reSpawnDistance = 400f;

    private List<GameObject> activeClouds = new List<GameObject>();
    private Queue<GameObject> cloudPool = new Queue<GameObject>();

    private bool initialized = false;

    private const float minYThreshold = -10f; // これより下には生成しない

    void OnEnable()
    {
        GameManager.OnInGamePlayerSpawned += OnPlayerSpawned;
    }

    void OnDisable()
    {
        GameManager.OnInGamePlayerSpawned -= OnPlayerSpawned;
    }

    void Start()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            GameObject prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
            GameObject cloud = Instantiate(prefab);
            cloud.SetActive(false);
            cloudPool.Enqueue(cloud);
        }
    }

    void Update()
    {
        if (playerPosition == null || !initialized) return;

        while (activeClouds.Count < cloudCount)
        {
            SpawnInitialCloud();
        }

        for (int i = activeClouds.Count - 1; i >= 0; i--)
        {
            GameObject cloud = activeClouds[i];
            float distance = Vector2.Distance(cloud.transform.position, playerPosition.position);

            if (distance > spawnRadius)
            {
                cloud.SetActive(false);
                cloudPool.Enqueue(cloud);
                activeClouds.RemoveAt(i);

                SpawnCloudOutsideRadius();
            }
        }
    }

    void SpawnInitialCloud()
    {
        if (cloudPool.Count == 0) return;

        GameObject cloud = cloudPool.Dequeue();

        Vector2 offset;
        float y;
        int attempts = 0;

        do
        {
            offset = Random.insideUnitCircle * spawnRadius;
            y = playerPosition.position.y + offset.y;
            attempts++;
        } while (y < minYThreshold && attempts < 10); // y < -10 を回避

        cloud.transform.position = new Vector3(playerPosition.position.x + offset.x, y, 0f);
        cloud.SetActive(true);
        activeClouds.Add(cloud);
    }

    void SpawnCloudOutsideRadius()
    {
        if (cloudPool.Count == 0) return;

        GameObject cloud = cloudPool.Dequeue();

        Vector2 offset;
        float y;
        int attempts = 0;

        do
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(spawnRadius + 50f, reSpawnDistance);
            offset = direction * distance;
            y = playerPosition.position.y + offset.y;
            attempts++;
        } while (y < minYThreshold && attempts < 10); // y < -10 を回避

        cloud.transform.position = new Vector3(playerPosition.position.x + offset.x, y, 0f);
        cloud.SetActive(true);
        activeClouds.Add(cloud);
    }

    private void OnPlayerSpawned(CustomPlayer spawnedPlayer)
    {
        playerPosition = spawnedPlayer.transform;
        initialized = true;

        for (int i = 0; i < cloudCount; i++)
        {
            SpawnInitialCloud();
        }
    }
}
