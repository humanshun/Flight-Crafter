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
    [SerializeField] private GameObject maxDistanceText; // 最高距離表示用テキスト
    [SerializeField] private GameObject maxAltitudeText; // 最高高度表示用テキスト
    [SerializeField] private GameObject distanceCrown;
    [SerializeField] private GameObject altitudeCrown;

    void Start()
    {
        GameManager.Instance.GameOverPopup(this);
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // 初期状態では非表示
        }
    }

    public void Show(bool distanceUpdated, bool altitudeUpdated)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            UpdateScoreDisplay();

            distanceCrown.SetActive(distanceUpdated);
            maxDistanceText.SetActive(distanceUpdated);
            altitudeCrown.SetActive(altitudeUpdated);
            maxAltitudeText.SetActive(altitudeUpdated);
        }
    }

    private void UpdateScoreDisplay()
    {
        if (distanceText != null)
        {
            distanceText.text = $"{score.distance:F1}m";
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
