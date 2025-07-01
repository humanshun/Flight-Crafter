using UnityEngine;
using System.Collections;

public class AddCoinEfect : MonoBehaviour
{
    [SerializeField] private CoinDisplay coinDisplay;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinPosition;
    [SerializeField] private Transform addCoinPosition;
    [SerializeField] private float spawnRadius = 20.0f;
    [SerializeField] private float moveSpeed = 5.0f;

    public void AddCoin(int earnedCoins)
    {
        StartCoroutine(SpawnCoins(earnedCoins));
        coinDisplay.AnimateAddCoins();
    }

    private IEnumerator SpawnCoins(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector2 spawnPos = (Vector2)coinPosition.position + randomOffset;

            float randomRotation = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, randomRotation);

            GameObject coin = Instantiate(coinPrefab, spawnPos, rotation, canvas);

            CoinMover mover = coin.AddComponent<CoinMover>();
            mover.Init(addCoinPosition, 1);

            // ← ここがポイント！ 出現間隔（たとえば 0.05秒おき）
            yield return new WaitForSeconds(0.05f);
        }
    }
}
