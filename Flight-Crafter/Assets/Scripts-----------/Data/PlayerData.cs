using UnityEngine;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

// プレイヤーのデータを管理するシングルトンクラス
public class PlayerData : MonoBehaviour
{
    // シングルトンインスタンス
    public static PlayerData Instance;

    // プレイヤーの所持コイン数
    public int playerCoins;

    // コインが変化したときに発火するイベント
    public static event System.Action<int> OnCoinsChanged;

    // 購入済みパーツのリスト（パーツ名で管理）
    private List<string> purchasedParts = new List<string>();

    // 現在装備しているパーツの名前
    private Dictionary<PartType, string> currentParts = new();

    // セーブファイルの保存先パス
    private string SavePath => Path.Combine(Application.persistentDataPath, "PlayerData_save.json");

    void Awake()
    {
        // シングルトンパターン：すでに存在していれば自分を破棄、いなければ自分をInstanceとして残す
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする
            LoadPlayerData(); // 起動時に保存データを読み込む
        }
        else
        {
            Destroy(gameObject); // すでに存在しているなら自分を削除
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ResetPlayerData(); // F1キーでデータをリセット
        }
    }

    // パーツ購入を試みる処理（成功ならtrue）
    public bool TryBuyPart(PartData part, int price)
    {
        if (playerCoins >= price && !purchasedParts.Contains(part.partName))
        {
            playerCoins -= price; // コインを減らす
            purchasedParts.Add(part.partName); // 購入済みリストに追加（重複チェックなし）
            SavePlayerData(); // データを保存
            Debug.Log("購入成功");

            // イベントを発火
            OnCoinsChanged?.Invoke(playerCoins);

            return true;
        }
        else
        {
            Debug.Log("購入失敗");
            return false;
        }
    }

    // 購入済みリストに追加する（重複を防ぐ）
    public void SavePurchasedPart(PartData part)
    {
        if (!purchasedParts.Contains(part.partName))
        {
            purchasedParts.Add(part.partName);
            SavePlayerData(); // 保存を反映
        }
    }

    // 現在装備しているパーツを保存（パーツ名だけ記録）
    public void SaveCurrentPart(PartData part)
    {
        currentParts[part.partType] = part.resaurceFileName;
        SavePlayerData(); // 保存を反映
    }

    public void RemoveCurrentPart(PartType partType)
    {
        if (currentParts.ContainsKey(partType))
        {
            currentParts.Remove(partType);
            SavePlayerData(); // 保存を反映
        }
    }

    public Dictionary<PartType, string> GetAllCurrentParts()
    {
        return new Dictionary<PartType, string>(currentParts);
    }

    public string GetCurrentPartName(PartType partType)
    {
        return currentParts.TryGetValue(partType, out var partName) ? partName : null;
    }

    // プレイヤーデータをJSONで保存
    private void SavePlayerData()
    {
        var saveData = new PlayerSaveData
        {
            coins = playerCoins,
            purchasedPartNames = purchasedParts,
            currentParts = new List<PartTypePartPair>()
        };

        foreach (var kvp in currentParts)
        {
            saveData.currentParts.Add(new PartTypePartPair { partType = kvp.Key, partName = kvp.Value });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("セーブデータを保存しました: " + SavePath);
    }

    // プレイヤーデータをJSONファイルから読み込み
    public void LoadPlayerData()
    {
        if (File.Exists(SavePath)) // セーブファイルが存在するか確認
        {
            string json = File.ReadAllText(SavePath); // ファイルを読み込む
            PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(json); // JSONをデシリアライズ

            // 読み込んだデータを反映
            playerCoins = saveData.coins;
            purchasedParts = saveData.purchasedPartNames ?? new List<string>();

            currentParts = new Dictionary<PartType, string>();
            if (saveData.currentParts != null)
            {
                foreach (var pair in saveData.currentParts)
                {
                    currentParts[pair.partType] = pair.partName; // パーツ名を保存
                }
            }
        }
        else
        {
            // セーブデータがない場合は初期化
            playerCoins = 2000;
            purchasedParts = new List<string>();
            currentParts = new Dictionary<PartType, string>();

            // イベントを発火
            OnCoinsChanged?.Invoke(playerCoins);
        }
    }
    public bool IsPartPurchased(string partName)
    {
        return purchasedParts.Contains(partName);
    }

    public void ResetPlayerData()
    {
        playerCoins = 10000;

        purchasedParts = new List<string>();
        currentParts = new Dictionary<PartType, string>();

        if (File.Exists(SavePath))
        {
            File.Delete(SavePath); // セーブファイルを削除
            Debug.Log("セーブデータを削除しました。");
        }
        SavePlayerData(); // 初期化後に保存
        Debug.Log("プレイヤーデータを初期化しました。");

        // イベントを発火
        OnCoinsChanged?.Invoke(playerCoins);
    }

    public void AddCoins(int amount)
    {
        if (amount < 0) return; // 負の値は無視

        playerCoins += amount;
        SavePlayerData();

        Debug.Log($"コインを {amount} 枚加算しました。現在の所持コイン: {playerCoins}");

        // イベントを発火
        OnCoinsChanged?.Invoke(playerCoins);
    }
}
