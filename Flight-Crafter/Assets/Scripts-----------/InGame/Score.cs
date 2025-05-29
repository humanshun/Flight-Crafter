using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Transform playerPosition;  // プレイヤーのTransform
    public float distance;            // 横方向の移動距離
    public float altitude;            // 縦方向の高度
    public float maxAltitude = 0f;  // 最高高度

    [SerializeField] private TextMeshProUGUI distanceText;  // 距離表示UI
    [SerializeField] private TextMeshProUGUI altitudeText;  // 高度表示UI
    [SerializeField] private GameObject coinText;      // コイン表示UI
    [SerializeField] private float startX = 250f;           // 計測開始X座標

    private bool hasStarted = false;  // 計測開始済みフラグ
    private float startPosX;          // 距離計測開始X座標

    void OnEnable()
    {
        GameManager.OnInGamePlayerSpawned += OnPlayerSpawned;
    }

    void OnDisable()
    {
        GameManager.OnInGamePlayerSpawned -= OnPlayerSpawned;
    }

    void Start()
    {
        GameManager.Instance.RegisterScore(this);
        coinText.SetActive(false); // 初期状態ではコイン表示を非表示にする
    }

    private void OnPlayerSpawned(CustomPlayer spawnedPlayer)
    {
        playerPosition = spawnedPlayer.transform;
        hasStarted = false;
        distance = 0f;
        altitude = 0f;
        distanceText.text = "距離: 0.0 m";
        altitudeText.text = "高度: 0.0 m";
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
    }

    public int CalculateCoins()
    {
        return Mathf.FloorToInt(distance / 10f); // 例：1m = 5コイン
    }

    public void OnGameOver()
    {
        distanceText.gameObject.SetActive(false);
        altitudeText.gameObject.SetActive(false);
        coinText.SetActive(true); // ゲームオーバー時にコイン表示を有効化
    }
}
