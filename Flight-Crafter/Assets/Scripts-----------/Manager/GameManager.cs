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

    void Awake()
    {
        // シングルトンパターン：すでに存在していれば自分を破棄、いなければ自分をInstanceとして残す
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする
            SceneManager.sceneLoaded += OnSceneLoaded; // シーンが読み込まれたときのイベントを登録
        }
        else
        {
            Destroy(gameObject); // すでに存在しているなら自分を削除
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
        SceneManager.LoadScene("Custom");
    }
}
