using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddCoinEfect : MonoBehaviour
{
    [SerializeField] private CoinDisplay coinDisplay;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinPosition;
    [SerializeField] private Transform addCoinPosition;
    [SerializeField] private float spawnRadius = 20.0f;

    private List<CoinMover> activeCoins = new List<CoinMover>();
    private bool isSkipping = false;
    void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        SkipAllAnimations(); // ← スキップ呼び出し
    }
}

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

            GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), canvas);
            CoinMover mover = coin.AddComponent<CoinMover>();

            mover.Init(addCoinPosition, 1f, OnCoinArrived);
            activeCoins.Add(mover);

            if (!isSkipping)
                yield return new WaitForSeconds(0.05f); // 通常は時間差で出す
        }
    }

    // スキップ処理：すべて即ゴールに
    public void SkipAllAnimations()
    {
        isSkipping = true;

        coinDisplay.SkipCoinAnimation();

        // ここでコピーしてからループする（元のリストが変更されても安全）
        foreach (var coin in activeCoins.ToArray())
        {
            if (coin != null && !coin.IsCompleted)
                coin.SkipToTarget();
        }

        activeCoins.Clear();
    }


    private void OnCoinArrived(CoinMover coin)
    {
        activeCoins.Remove(coin);
    }
}
