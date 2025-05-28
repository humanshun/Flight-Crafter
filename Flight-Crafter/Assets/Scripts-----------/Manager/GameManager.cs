using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CustomPlayer player;
    private CustomPlayer playerInstance;
    public CustomPlayer Player => playerInstance;

    [SerializeField] private CustomPlayer inGamePlayer;
    private CustomPlayer inGamePlayerInstance;
    public CustomPlayer InGamePlayer => inGamePlayerInstance;

    // シングルトンインスタンス
    public static GameManager Instance;

    public static event System.Action<CustomPlayer> OnInGamePlayerSpawned;

    private Score score;

    public GameOverPopup gameOvarPopup;
    public bool isGameOver = false;

    void Start()
    {
        // すでにCustomシーンが読み込まれていた場合に備える
        if (SceneManager.GetActiveScene().name == "Custom")
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        else if (SceneManager.GetActiveScene().name == "InGame")
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

    public void RegisterScore(Score s)
    {
        score = s;
    }

    public void GameOverPopup(GameOverPopup popup)
    {
        gameOvarPopup = popup;
    }


    void Awake()
    {
        // シングルトンパターン：すでに存在していれば自分を破棄、いなければ自分をInstanceとして残す
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする

            // ↓ この登録を一度だけ行う
            SceneManager.sceneLoaded -= OnSceneLoaded; // ← まず念のため削除
            SceneManager.sceneLoaded += OnSceneLoaded; // シーンが読み込まれたときのイベントを登録
        }
        else
        {
            Destroy(gameObject); // すでに存在しているなら自分を削除
        }
    }
    void OnDestroy()
    {
        // シーンが破棄されるときにイベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isGameOver = false; // シーンが読み込まれたらゲームオーバー状態をリセット

        if (scene.name == "Custom")
        {
            Vector3 spawnPosition = new Vector3(-3.7f, -1.15f, 0f);

            if (playerInstance != null)
            {
                Destroy(playerInstance.gameObject); // 既存のインスタンスを削除
            }
            playerInstance = Instantiate(player, spawnPosition, Quaternion.identity);
        }
        else if (scene.name == "InGame")
        {
            Vector3 spawnPosition = new Vector3(-3.7f, -30f, 0f);

            if (inGamePlayerInstance != null)
            {
                Destroy(inGamePlayerInstance.gameObject); // 既存のインスタンスを削除
            }
            inGamePlayerInstance = Instantiate(inGamePlayer, spawnPosition, Quaternion.identity);
            OnInGamePlayerSpawned?.Invoke(inGamePlayerInstance); // イベントを発火して、InGamePlayerが生成されたことを通知
        }
    }

    public void GameOver()
    {
        if (isGameOver) return; // すでにゲームオーバーなら何もしない
        isGameOver = true; // ゲームオーバー状態にする

        //もしscoreがnullで無ければ、Coinsを計算して追加
        int earnedCoins = score != null ? score.CalculateCoins() : 0;
        PlayerData.Instance.AddCoins(earnedCoins);
        Debug.Log($"コイン獲得: {earnedCoins}");

        bool distanceUpdated = false;
        bool altitudeUpdated = false;

        //ハイスコア記録
        if (score != null)
        {
            (distanceUpdated, altitudeUpdated) = PlayerData.Instance.TryUpdateHighScore(score.distance, score.maxAltitude);
        }

        // ゲームオーバーのポップアップを表示
        if (gameOvarPopup != null)
        {
            gameOvarPopup.Show(distanceUpdated, altitudeUpdated);
        }

        //scoreにゲーム終了したことを通知
        if (score != null)
        {
            score.OnGameOver();
        }
    }
}
