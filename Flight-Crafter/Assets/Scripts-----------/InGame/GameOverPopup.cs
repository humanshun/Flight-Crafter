using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // ゲームオーバーパネル
    [SerializeField] private Button restartButton; // 再スタートボタン
    [SerializeField] private Button exitButton; // ゲーム終了ボタン

    [SerializeField] private Score score; // スコア管理用スクリプト
    [SerializeField] private TextMeshProUGUI distanceText; // 距離表示用テキスト
    [SerializeField] private TextMeshProUGUI altitudeText; // 高度表示用テキスト
    [SerializeField] private TextMeshProUGUI coinText; // コイン表示用テキスト

    void Start()
    {
        GameManager.Instance.GameOverPopup(this);
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // 初期状態では非表示
        }
    }

    public void Show()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            UpdateScoreDisplay();
        }
    }

    private void UpdateScoreDisplay()
    {
        Debug.Log("a");
        if (distanceText != null)
        {
            distanceText.text = $"{score.distance:F1}m";
            Debug.Log("距離表示");
        }
        if (altitudeText != null)
        {
            altitudeText.text = $"{score.maxAltitude:F1}m";
        }
        if (coinText != null)
        {
            coinText.text = $"{score.CalculateCoins()}Coins";
        }
    }
}
