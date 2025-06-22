using Ricimi;
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

    private InGameUI score;

    private Canvas canvas;
    public GameOverPopup gameOvarPopup;
    public TutorialCustom1 tutorialCustomPopup1;
    public TutorialInGame tutorialInGamePopup;
    public bool isGameOver = false;

    //ゲーム進行フラグ
    public bool isTutorialCustom = false;
    public bool isTutorialInGame = false;
    public bool isBuyPart = false;
    public bool isChangePart = false;
    public bool isClearTutorial = false;
    public bool isClearInGameTutorial = false;

    public void RegisterScore(InGameUI s)
    {
        score = s;
    }

    public void GameOverPopup(GameOverPopup popup)
    {
        gameOvarPopup = popup;
    }
    public void TutorialCustomPopup1(TutorialCustom1 popup)
    {
        tutorialCustomPopup1 = popup;
    }

    public void TutorialInGamePopup(TutorialInGame popup)
    {
        tutorialInGamePopup = popup;
    }
    public void Canvas(Canvas inCanvas)
    {
        canvas = inCanvas;
    }
    void Awake()
    {
        // シングルトンパターン：すでに存在していれば自分を破棄、いなければ自分をInstanceとして残す
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする
            PlayerData.OnPartPurchased += HandlePartPurchased;
            PlayerData.OnEquippedNonInitialPart += HandleChangePart;
        }
        else
        {
            Destroy(gameObject); // すでに存在しているなら自分を削除
        }
    }
    void OnDestroy()
    {
        // シーンが破棄されるときにイベントを解除
        PlayerData.OnPartPurchased -= HandlePartPurchased;
        PlayerData.OnEquippedNonInitialPart -= HandleChangePart;
    }
    public void TutorialShow(Scene scene)
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

            if (!isTutorialCustom)
            {
                isTutorialCustom = true;
                tutorialCustomPopup1.gameObject.SetActive(true); // チュートリアルポップアップを表示
                tutorialCustomPopup1.DisableAllButtons();
                isTutorialCustom = true; // チュートリアルが開始されたことを記録
            }
            else
            {
                tutorialCustomPopup1.gameObject.SetActive(false);
            }
        }
        else if (scene.name == "InGame")
        {
            Vector3 spawnPosition = new Vector3(-3.7f, -32f, 0f);

            if (inGamePlayerInstance != null)
            {
                Destroy(inGamePlayerInstance.gameObject); // 既存のインスタンスを削除
            }
            inGamePlayerInstance = Instantiate(inGamePlayer, spawnPosition, Quaternion.identity);
            OnInGamePlayerSpawned?.Invoke(inGamePlayerInstance); // イベントを発火して、InGamePlayerが生成されたことを通知
            if (tutorialInGamePopup != null)
            {
                tutorialInGamePopup.gameObject.SetActive(false); // チュートリアルポップアップを非表示にする
            }

            if (!isTutorialInGame)
            {
                isTutorialInGame = true; // チュートリアルが開始されたことを記録
                tutorialInGamePopup.gameObject.SetActive(true);
            }
        }
    }

    public void GameOver()
    {
        if (isGameOver) return; // すでにゲームオーバーなら何もしない
        isGameOver = true; // ゲームオーバー状態にする

        //もしscoreがnullで無ければ、Coinsを計算して追加
        int earnedCoins = score != null ? score.CalculateCoins() : 0;
        PlayerData.Instance.AddCoins(earnedCoins);

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

    public void GameClear()
    {
        if (isGameOver) return;
        isGameOver = true;
        Transition.LoadLevel("Clear", 2.0f, Color.black);
        Debug.Log("ゲームクリア");
    }
    private void HandlePartPurchased()
    {
        isBuyPart = true;
    }

    private void HandleChangePart()
    {
        isChangePart = true;
    }
}
