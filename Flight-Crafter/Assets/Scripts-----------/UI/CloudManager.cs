using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public Transform playerPosition;  // プレイヤーの位置
    public GameObject[] cloudPrefabs;  // 雲のプレハブ配列
    private float cloudActiveRange = 200f;  // 雲がアクティブである範囲
    private float cloudSpacing = 20f;  // 雲を均等に配置する間隔
    private float minHeight = -10f;  // 雲の最小高さ
    private float maxHeight = 200f;  // 雲の最大高さ
    private List<GameObject> clouds = new List<GameObject>();  // 生成した雲を格納するリスト

    void Start()
    {
        // 最初に雲を配置する
        GenerateClouds();
    }

    void Update()
    {
        // プレイヤーの位置に基づいて雲を管理
        ManageClouds();
    }

    void GenerateClouds()
    {
        // 最初に、プレイヤーの位置を基準に雲を生成する
        float startPositionX = playerPosition.position.x - cloudActiveRange;

        // 雲を均等に配置
        for (float x = startPositionX; x < playerPosition.position.x + cloudActiveRange; x += cloudSpacing)
        {
            float randomY = Random.Range(minHeight, maxHeight);  // 高さをランダムに設定
            GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];  // ランダムに雲を選ぶ
            GameObject cloud = Instantiate(cloudPrefab, new Vector3(x, randomY, 0f), Quaternion.identity);
            clouds.Add(cloud);  // 生成した雲をリストに追加
        }
    }

    void ManageClouds()
    {
        // 現在画面外に出た雲を削除し、新たに生成する
        for (int i = clouds.Count - 1; i >= 0; i--)
        {
            GameObject cloud = clouds[i];

            // 雲が指定の範囲外に出た場合
            if (Mathf.Abs(cloud.transform.position.x - playerPosition.position.x) > cloudActiveRange)
            {
                // 雲を削除
                Destroy(cloud);
                clouds.RemoveAt(i);

                // 新たに雲を生成してリストに追加
                float randomY = Random.Range(minHeight, maxHeight);  // 高さをランダムに設定
                GameObject cloudPrefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];  // ランダムに雲を選ぶ
                GameObject newCloud = Instantiate(cloudPrefab, new Vector3(playerPosition.position.x + cloudActiveRange, randomY, 0f), Quaternion.identity);
                clouds.Add(newCloud);  // 生成した雲をリストに追加
            }
        }
    }
}
