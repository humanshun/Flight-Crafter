using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private const string PlayerCoinsKey = "PlayerCoins";
    public int playerCoins;

    void Start()
    {
        // PlayerPrefs.DeleteAll();
        // PlayerPrefsからコインのデータを読み込む
        playerCoins = 2000;
        SavePlayerCoins();
    }
    public void SavePlayerCoins()
    {
        // PlayerPrefsにコインのデータを保存する
        PlayerPrefs.SetInt(PlayerCoinsKey, playerCoins);
        PlayerPrefs.Save();
    }
}