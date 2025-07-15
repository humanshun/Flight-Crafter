using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class AddCoinEffect : MonoBehaviour
{
    [SerializeField] private CoinDisplay coinDisplay;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinPosition;
    [SerializeField] private Transform addCoinPosition;
    [SerializeField] private float spawnRadius = 20.0f;

    private List<CoinMover> activeCoins = new List<CoinMover>();
    private bool isSkipping = false;

    // UniTask用のキャンセル管理
    private CancellationTokenSource spawnCts;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SkipAllAnimations();
        }
    }

    public void AddCoin(int earnedCoins)
    {
        // 前回のタスクが動いてたら止める
        spawnCts?.Cancel();
        spawnCts = new CancellationTokenSource();

        // UniTask版スポーン
        SpawnCoinsAsync(earnedCoins, spawnCts.Token).Forget();

        // コインUI演出
        coinDisplay.AnimateAddCoins();
    }

    private async UniTaskVoid SpawnCoinsAsync(int count, CancellationToken token)
    {
        for (int i = 0; i < count; i++)
        {
            if (token.IsCancellationRequested || isSkipping) break;

            // ランダム位置に生成
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector2 spawnPos = (Vector2)coinPosition.position + randomOffset;

            GameObject coin = Instantiate(
                coinPrefab,
                spawnPos,
                Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)),
                canvas
            );

            CoinMover mover = coin.AddComponent<CoinMover>();
            mover.Init(addCoinPosition, 1f, OnCoinArrived);
            activeCoins.Add(mover);

            // スキップしてない場合だけ待つ
            if (!isSkipping)
            {
                try
                {
                    await UniTask.Delay(50, cancellationToken: token);
                }
                catch
                {
                    // キャンセルされたら即終了
                    break;
                }
            }
        }
    }

    public void SkipAllAnimations()
    {
        if (isSkipping) return; // 二重スキップ防止
        isSkipping = true;

        // コインUI演出も即終了
        coinDisplay.SkipCoinAnimation();

        // すべての飛んでるコインを即ゴールへ
        foreach (var coin in activeCoins.ToArray())
        {
            if (coin != null && !coin.IsCompleted)
                coin.SkipToTarget(); // 即到着するように実装済みならここで終わる
        }

        activeCoins.Clear(); // もう演出なし
    }

    private void OnCoinArrived(CoinMover coin)
    {
        activeCoins.Remove(coin);
    }
}
