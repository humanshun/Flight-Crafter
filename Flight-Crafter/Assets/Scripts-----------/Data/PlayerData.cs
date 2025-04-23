using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    private const string PlayerCoinsKey = "PlayerCoins";
    public int playerCoins;

    void Awake()
    {
        // 同じPlayerDataがあれば破棄（シングルトンにする）
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playerCoins = PlayerPrefs.GetInt(PlayerCoinsKey, 2000);
    }

    public void SavePlayerCoins()
    {
        // PlayerPrefsにコインのデータを保存する
        PlayerPrefs.SetInt(PlayerCoinsKey, playerCoins);
        PlayerPrefs.Save();
    }
    public bool TryBuyPart(int price)
    {
        if (playerCoins >= price)
        {
            playerCoins -= price; // コインを減らす
            SavePlayerCoins(); // セーブする
            return true; // 購入成功
        }
        else
        {
            Debug.Log("Not enough coins to buy this part.");
            return false; // 購入失敗
        }
    }
}