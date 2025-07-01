using TMPro;
using UnityEngine;
using System.Collections;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private AddCoinEfect addCoinEfect;

    [SerializeField] private float interval = 0.08f; // 1枚加算するごとの間隔
    [SerializeField] private float startDelay = 1.55f;

    private int currentCoin = 0;
    public int earnedCoins = 0;

    void Start()
    {
        currentCoin = PlayerData.Instance.playerCoins;
        coinText.text = currentCoin.ToString();
    }

    public void AnimateAddCoins()
    {
        StartCoroutine(AddCoinsRoutine());
    }

    private IEnumerator AddCoinsRoutine()
    {
        yield return new WaitForSeconds(startDelay); // 開始前に待機

        int targetCoin = currentCoin + earnedCoins;

        while (currentCoin < targetCoin)
        {
            currentCoin++;
            coinText.text = currentCoin.ToString();
            yield return new WaitForSeconds(interval);
        }

        // 念のため最終値をセット（途中スキップとかの対策）
        coinText.text = targetCoin.ToString();
    }
}
