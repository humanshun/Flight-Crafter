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
                AddStatus(body.weight.displayName, body.weight.value);
                AddStatus(body.airResistance.displayName, body.airResistance.value);
                break;

            case PartType.Rocket:
                RocketData rocket = (RocketData)part;
                AddStatus(rocket.weight.displayName, rocket.weight.value);
                AddStatus(rocket.jetThrust.displayName, rocket.jetThrust.value);
                AddStatus(rocket.jetTime.displayName, rocket.jetTime.value);
                break;

            case PartType.Tire:
                TireData tire = (TireData)part;
                AddStatus(tire.weight.displayName, tire.weight.value);
                AddStatus(tire.torque.displayName, tire.torque.value);
                break;

            case PartType.Wing:
                WingData wing = (WingData)part;
                AddStatus(wing.weight.displayName, wing.weight.value);
                AddStatus(wing.lift.displayName, wing.lift.value);
                AddStatus(wing.airControl.displayName, wing.airControl.value);
                AddStatus(wing.airRotationalControl.displayName, wing.airRotationalControl.value);
                AddStatus(wing.propulsionPower.displayName, wing.propulsionPower.value);
                break;
        }

        partSetButton.onClick.RemoveAllListeners(); // 既存のリスナーを削除
        partSetButton.onClick.AddListener(() => ButtonClick(part, currentPartPopup));
    }

    private void AddStatus(string displayName, float value)
    {
        status = Instantiate(statusBar.gameObject, ContentTransform);
        StatusBar statusBarInstance = status.GetComponent<StatusBar>();
        statusBarInstance.Setup(displayName, value);
    }


    public void Hide()
    {
        popupRoot.SetActive(false);
    }

    public void ButtonClick(PartData part, CurrentPartPopup currentPartPopup)
    {
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
