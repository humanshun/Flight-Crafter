using UnityEngine;

public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static GameManager Instance;

    // プレイヤーのデータを管理するクラス
    public PlayerData playerData;

    void Awake()
    {
        // シングルトンパターン：すでに存在していれば自分を破棄、いなければ自分をInstanceとして残す
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする
        }
        else
        {
            Destroy(gameObject); // すでに存在しているなら自分を削除
        }
    }

    void Start()
    {
        playerData.LoadPlayerData(); // 起動時に保存データを読み込む
    }

    public void ResetPlayerData()
    {
        playerData.ResetPlayerData(); // データをリセット
    }
}
