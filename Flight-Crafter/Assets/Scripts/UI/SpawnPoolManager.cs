using UnityEngine;
using System.Collections.Generic;

// スポーン確率付きのPrefabデータ
[System.Serializable]
public class SpawnablePrefab
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnProbability = 1f;
}

public class SpawnPoolManager : MonoBehaviour
{
    public Transform playerPosition;
    public SpawnablePrefab[] spawnPrefabs;

    [Header("表示設定")]
    public int prefabCount = 50;
    public float spawnRadius = 500f;
    public float reSpawnDistance = 400f;

    private List<GameObject> activeClouds = new List<GameObject>();
    private Queue<GameObject> prefabPool = new Queue<GameObject>();

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
        for (int i = 0; i < prefabCount; i++)
        {
            GameObject prefab = GetRandomPrefabByProbability();
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            prefabPool.Enqueue(obj);
        }
    }

    void Update()
    {
        if (playerPosition == null || !initialized) return;

        int maxSpawnAttempts = 100; // 無限ループ防止
        int spawnAttempts = 0;
        while (activeClouds.Count < prefabCount && spawnAttempts < maxSpawnAttempts)
        {
            SpawnInitialCloud();
            spawnAttempts++;
        }

        for (int i = activeClouds.Count - 1; i >= 0; i--)
        {
            GameObject obj = activeClouds[i];
            if (obj == null) // ★追加：破棄されたオブジェクトの対策
            {
                activeClouds.RemoveAt(i);
                continue;
            }
            float distance = Vector2.Distance(obj.transform.position, playerPosition.position);

            if (distance > spawnRadius)
            {
                obj.SetActive(false);
                prefabPool.Enqueue(obj);
                activeClouds.RemoveAt(i);

                SpawnCloudOutsideRadius();
            }
        }
    }

    void SpawnInitialCloud()
    {
        if (prefabPool.Count == 0) return;

        GameObject obj = prefabPool.Dequeue();

        Vector2 offset;
        float y;
        int attempts = 0;
        bool found = false;

        do
        {
            offset = Random.insideUnitCircle * spawnRadius;
            y = playerPosition.position.y + offset.y;
            attempts++;
            if (y >= minYThreshold) found = true;
        } while (!found && attempts < 10);

        if (!found)
        {
            prefabPool.Enqueue(obj); // 位置が見つからなければ戻す
            return;
        }

        obj.transform.position = new Vector3(playerPosition.position.x + offset.x, y, 0f);
        obj.SetActive(true);
        activeClouds.Add(obj);
    }

    void SpawnCloudOutsideRadius()
    {
        if (prefabPool.Count == 0) return;

        GameObject obj = prefabPool.Dequeue();

        Vector2 playerDir = Vector2.right; // デフォルトは右向き
        if (playerPosition.TryGetComponent<Rigidbody2D>(out var rb))
        {
            if (rb.linearVelocity.sqrMagnitude > 0.1f)
                playerDir = rb.linearVelocity.normalized;
        }

        Vector2 offset;
        float y;
        int attempts = 0;

        do
        {
            // 進行方向±60度くらいの範囲で雲を出す
            float angle = Mathf.Atan2(playerDir.y, playerDir.x) + Random.Range(-Mathf.PI / 3, Mathf.PI / 3);
            float distance = Random.Range(spawnRadius + 50f, reSpawnDistance);
            offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
            y = playerPosition.position.y + offset.y;
            attempts++;
        } while (y < minYThreshold && attempts < 10);

        // Prefabが変わる場合はここで差し替え（再利用時は不要）
        // cloud = ReplaceCloudPrefabIfNeeded(cloud);

        obj.transform.position = new Vector3(playerPosition.position.x + offset.x, y, 0f);
        obj.SetActive(true);
        activeClouds.Add(obj);
    }

    // 確率に応じてPrefabを選ぶ
    private GameObject GetRandomPrefabByProbability()
    {
        float total = 0f;
        foreach (var p in spawnPrefabs) total += p.spawnProbability;
        float r = Random.Range(0f, total);
        float accum = 0f;
        foreach (var p in spawnPrefabs)
        {
            accum += p.spawnProbability;
            if (r <= accum) return p.prefab;
        }
        return spawnPrefabs[spawnPrefabs.Length - 1].prefab; // フォールバック
    }

    private void OnPlayerSpawned(CustomPlayer spawnedPlayer)
    {
        playerPosition = spawnedPlayer.transform;
        initialized = true;

        for (int i = 0; i < prefabCount; i++)
        {
            SpawnInitialCloud();
        }
    }
}
