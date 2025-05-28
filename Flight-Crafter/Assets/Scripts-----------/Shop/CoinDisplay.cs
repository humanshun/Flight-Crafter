using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    void OnEnable()
    {
        // イベント登録
        PlayerData.OnCoinsChanged += UpdateCoinDisplay;
    }

    void OnDisable()
    {
        // イベント解除（忘れずに！）
        PlayerData.OnCoinsChanged -= UpdateCoinDisplay;
    }

    void Start()
    {
        // 初期表示
        coinText.text = PlayerData.Instance.playerCoins.ToString();
    }

    private void UpdateCoinDisplay(int newCoinValue)
    {
        coinText.text = newCoinValue.ToString();
    }
}
