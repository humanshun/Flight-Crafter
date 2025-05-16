using TMPro;
using UnityEngine;

public class DescriptionPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot; // ポップアップの本体
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform ContentTransform;
    [SerializeField] private StatusBar statusBar; // ステータス情報のプレハブ
    private GameObject status;

    public void Show(PartData part)
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
}
