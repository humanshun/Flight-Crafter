using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot; // ポップアップの本体
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform ContentTransform;
    [SerializeField] private StatusBar statusBar; // ステータス情報のプレハブ
    [SerializeField] private Button partSetButton; // パーツセットボタン
    [SerializeField] private CustomPlayer customPlayer; // プレイヤーのカスタムオブジェクト
    private GameObject status;
    public void Show(PartData part, CurrentPartPopup currentPartPopup)
    {
        customPlayer = GameManager.Instance.Player;

        popupRoot.SetActive(true);
        nameText.text = part.partName;

        // 古いステータスを削除
        foreach (Transform child in ContentTransform)
        {
            Destroy(child.gameObject);
        }

        // パーツタイプに応じて表示
        switch (part.partType)
        {
            case PartType.Body:
                BodyData body = (BodyData)part;
                AddStatus(PartType.Body, body.weight.displayName, body.weight.value);
                AddStatus(PartType.Body, body.hp.displayName, body.hp.value);
                AddStatus(PartType.Body, body.airResistance.displayName, body.airResistance.value);
                break;

            case PartType.Rocket:
                RocketData rocket = (RocketData)part;
                AddStatus(PartType.Rocket, rocket.weight.displayName, rocket.weight.value);
                AddStatus(PartType.Rocket, rocket.jetThrust.displayName, rocket.jetThrust.value);
                AddStatus(PartType.Rocket, rocket.jetTime.displayName, rocket.jetTime.value);
                break;

            case PartType.Tire:
                TireData tire = (TireData)part;
                AddStatus(PartType.Tire, tire.weight.displayName, tire.weight.value);
                AddStatus(PartType.Tire, tire.airResistance.displayName, tire.airResistance.value);
                AddStatus(PartType.Tire, tire.torque.displayName, tire.torque.value);
                break;

            case PartType.Wing:
                WingData wing = (WingData)part;
                AddStatus(PartType.Wing, wing.weight.displayName, wing.weight.value);
                AddStatus(PartType.Wing, wing.airResistance.displayName, wing.airResistance.value);
                AddStatus(PartType.Wing, wing.airControl.displayName, wing.airControl.value);
                break;
        }
        partSetButton.onClick.AddListener(() => ButtonClick(part, currentPartPopup));
    }

    private void AddStatus(PartType partType, string displayName, float value)
    {
        float maxValue = GetMaxValue(partType, displayName);
        status = Instantiate(statusBar.gameObject, ContentTransform);
        StatusBar statusBarInstance = status.GetComponent<StatusBar>();
        statusBarInstance.Setup(partType, displayName, value, maxValue);
    }

    private float GetMaxValue(PartType partType, string displayName)
    {
        switch (partType)
        {
            case PartType.Body: //TODO ディスプレイネームをenumに変更
                if (displayName == "重量") return 5f;
                if (displayName == "体力") return 100f;
                if (displayName == "空気抵抗") return 0.3f;
                break;
            case PartType.Rocket:
                if (displayName == "重量") return 5f;
                if (displayName == "噴射力") return 500f;
                if (displayName == "噴射時間") return 100f;
                break;
            case PartType.Tire:
                if (displayName == "重量") return 5f;
                if (displayName == "空気抵抗") return 0.3f;
                if (displayName == "地上加速度") return 50f;
                break;
            case PartType.Wing:
                if (displayName == "重量") return 5f;
                if (displayName == "空気抵抗") return 0.3f;
                if (displayName == "空中制御") return 100f;
                break;
        }
        return 1f; // デフォルトで割れないように1にしておく
    }


    public void Hide()
    {
        popupRoot.SetActive(false);
    }

    public void ButtonClick(PartData part, CurrentPartPopup currentPartPopup)
    {
        AudioManager.Instance.PlaySFX("SE_ButtonLow");
        string currentName = PlayerData.Instance.GetCurrentPartName(part.partType);
        if (!string.IsNullOrEmpty(currentName) && currentName != part.partName)
        {
            Debug.Log($"{part.partType}の現在の装備{currentName}を削除します");
            PlayerData.Instance.RemoveCurrentPart(part.partType);
        }

        PlayerData.Instance.SaveCurrentPart(part);
        currentPartPopup.Setup(part);

        customPlayer.SetupAll();
    }
}
