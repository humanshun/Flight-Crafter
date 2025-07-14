using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private AddCoinEfect addCoinEfect;

    [SerializeField] private float interval = 0.08f;
    [SerializeField] private float startDelay = 1.55f;

    private int currentCoin = 0;
    public int earnedCoins = 0;

    private bool isSkipping = false;
    private bool isAnimating = false;

    void Start()
    {
        currentCoin = PlayerData.Instance.playerCoins;
        coinText.text = currentCoin.ToString();
    }

    public void AnimateAddCoins()
    {
        if (isAnimating)
        {
            return;
        }

        isSkipping = false;
        _ = AddCoinsRoutineAsync(); // 非同期で実行
    }

    private async UniTaskVoid AddCoinsRoutineAsync()
    {
        isAnimating = true;

        await UniTask.Delay((int)(startDelay * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());

        int targetCoin = currentCoin + earnedCoins;

        if (isSkipping)
        {
            currentCoin = targetCoin;
            coinText.text = currentCoin.ToString();
        }
        else
        {
            while (currentCoin < targetCoin)
            {
                if (isSkipping)
                {
                    await UniTask.Delay((int)(startDelay * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
                    currentCoin = targetCoin;
                    coinText.text = currentCoin.ToString();
                    break;
                }

                currentCoin++;
                coinText.text = currentCoin.ToString();

                await UniTask.Delay((int)(interval * 1000), cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }

        isAnimating = false;
    }

    public void SkipCoinAnimation()
    {
        isSkipping = true;
    }

    public void AddCoinsImmediately(int amount)
    {
        currentCoin += amount;
        coinText.text = currentCoin.ToString();
    }
}
