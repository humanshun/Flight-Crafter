using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public PlayerData playerData; // プレイヤーデータの参照
    public PartData[] shopItems; // ショップアイテムのデータ配列
    public Button[] buyButtons; // 購入ボタンの配列

    void Start()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (i < buyButtons.Length && buyButtons[i] != null)
            {
                int index = i; // ローカル変数にインデックスを保存
                buyButtons[i].onClick.AddListener(() => BuyItem(shopItems[index]));
                buyButtons[i].GetComponentInChildren<Text>().text = shopItems[index].partName + " - " + shopItems[index].partCost + " Coins";
            }
        }
    }

    void BuyItem(PartData item)
    {
        if (playerData.playerCoins >= item.partCost)
        {
            playerData.playerCoins -= item.partCost;
            playerData.SavePlayerCoins(); // コインのデータを保存
            Debug.Log(item.partName + " 購入成功！ 残りコイン: " + playerData.playerCoins);
            // ここでアイテムをプレイヤーに追加する処理を行う
        }
        else
        {
            Debug.Log("コインが不足しています。");
        }
    }
}