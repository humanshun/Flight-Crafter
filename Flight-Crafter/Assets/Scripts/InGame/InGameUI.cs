using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private CustomPlayer player; // プレイヤーのインスタンス
    [SerializeField] private PlayerController2 playerController; // プレイヤーコントローラーのインスタンス
    [SerializeField] private Transform playerPosition;  // プレイヤーのTransform
    public float distance;            // 横方向の移動距離
    public float altitude;            // 縦方向の高度
    public float maxAltitude = 0f;  // 最高高度


    [SerializeField] private TextMeshProUGUI distanceText;  // 距離表示UI
    [SerializeField] private TextMeshProUGUI altitudeText;  // 高度表示UI
    [SerializeField] private CoinDisplay coinDisplay;      // コイン表示UI
    [SerializeField] private AddCoinEffect addCoinEffect;
    [SerializeField] private float startX = 250f;           // 計測開始X座標
    [SerializeField] private Slider slider; // スクロールバー
    private float goal = 7000f; // ゴール地点のX座標

    private bool hasStarted = false;  // 計測開始済みフラグ
    private float startPosX;          // 距離計測開始X座標

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider rocketSlider;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI rocketText;

    [SerializeField] private GameObject playerUI;

    private int collectionCoins = 0; // 収集したコインの数
    private float initialHealth = 1f; // 初期ヘルス
    private float initialRocketTime = 1f; // 初期ロケット時間



    void OnEnable()
    {
        GameManager.OnInGamePlayerSpawned += OnPlayerSpawned;
    }

    void OnDisable()
    {
        GameManager.OnInGamePlayerSpawned -= OnPlayerSpawned;

        if (playerController != null)
        {
            playerController.OnHealthChanged -= UpdateHealthUI;
            playerController.OnRocketTimeChanged -= UpdateRocketUI;
        }
    }

    public void Setup(float health, float rocket)
    {
        initialHealth = health;
        initialRocketTime = rocket;

        UpdateHealthUI(health);
        UpdateRocketUI(rocket);
    }

    void Start()
    {
        GameManager.Instance.RegisterScore(this);
        coinDisplay.gameObject.SetActive(false); // 初期状態ではコイン表示を非表示にする
    }

    private void OnPlayerSpawned(CustomPlayer spawnedPlayer)
    {
        playerPosition = spawnedPlayer.transform;
        hasStarted = false;
        distance = 0f;
        altitude = 0f;
        distanceText.text = "距離: 0.0 m";
        altitudeText.text = "高度: 0.0 m";

        playerController = spawnedPlayer.GetComponent<PlayerController2>();
        if (playerController != null)
        {
            Setup(playerController.TotalHealth, playerController.TotalRocketTime);

            // イベント購読
            playerController.OnHealthChanged += UpdateHealthUI;
            playerController.OnRocketTimeChanged += UpdateRocketUI;
        }
    }

    void Update()
    {
        if (playerPosition == null) return;

        altitude = Mathf.Max(0f, (playerPosition.position.y + 36f) / 5f);
        altitudeText.text = "高度: " + altitude.ToString("F1") + " m";

        // ✅ 最高高度更新
        if (altitude > maxAltitude)
        {
            maxAltitude = altitude;
        }


        if (!hasStarted)
        {
            // プレイヤーがstartXに達したら記録開始
            if (playerPosition.position.x >= startX)
            {
                startPosX = playerPosition.position.x;
                hasStarted = true;
            }
        }
        else
        {
            // 距離と高度をそれぞれ計算
            distance = Mathf.Max(0f, (playerPosition.position.x - startPosX) / 5f);
            distanceText.text = "距離: " + distance.ToString("F1") + " m";
        }

        // スクロールバーの値を更新
        if (slider != null)
        {
            slider.value = Mathf.Clamp01(playerPosition.position.x / goal);
        }


    }

    public void AddCollectedCoins(int coins)
    {
        collectionCoins += coins;
    }

    public int CalculateCoins()
    {
        int baseCoins = Mathf.FloorToInt(distance / 10f);
        return baseCoins + collectionCoins;
    }

    public void OnGameOver()
    {
        distanceText.gameObject.SetActive(false);
        altitudeText.gameObject.SetActive(false);
        coinDisplay.earnedCoins = CalculateCoins();
        coinDisplay.gameObject.SetActive(true); // ゲームオーバー時にコイン表示を有効化
        addCoinEffect.gameObject.SetActive(true);
        addCoinEffect.AddCoin(CalculateCoins());
        slider.gameObject.SetActive(false); // スクロールバーを非表示にする
        playerUI.SetActive(false); // プレイヤーUIを非表示にする
    }
    
    private void UpdateHealthUI(float health)
    {
        float percent = (health / initialHealth) * 100f;
        healthSlider.value = percent;
        healthText.text = percent.ToString("F0") + "%";
    }

    private void UpdateRocketUI(float rocket)
    {
        float percent = (rocket / initialRocketTime) * 100f;
        rocketSlider.value = percent;
        rocketText.text = percent.ToString("F0") + "%";
    }
}
