using TMPro;
using UnityEngine;

public class DescriptionPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot; // ポップアップの本体
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI[] statsText; // 任意

    public void Show(PartData part)
    {
        popupRoot.SetActive(true);
        nameText.text = part.partName;

        // 必要に応じてステータス情報も表示（例）
        statsText[0].text = $"{part.weight.displayName}:{part.weight.value}";

        switch (part.partType)
        {
            case PartType.Body:
                var body = part as BodyData;
                statsText[1].text += $"\n{body.airResistance.displayName}:{body.airResistance.value}";
                break;
            case PartType.Rocket:
                var rocket = part as RocketData;
                statsText[1].text += $"\n{rocket.jetThrust.displayName}:{rocket.jetThrust.value}";
                statsText[2].text += $"\n{rocket.jetTime.displayName}:{rocket.jetTime.value}";
                break;
            case PartType.Tire:
                var tire = part as TireData;
                statsText[1].text += $"\n{tire.torque.displayName}:{tire.torque.value}";
                break;
            case PartType.Wing:
                var wing = part as WingData;
                statsText[1].text += $"\n{wing.lift.displayName}:{wing.lift.value}";
                statsText[2].text += $"\n{wing.airControl.displayName}:{wing.airControl.value}";
                statsText[3].text += $"\n{wing.airRotationalControl.displayName}:{wing.airRotationalControl.value}";
                break;
        }
    }

    public void Hide()
    {
        popupRoot.SetActive(false);
    }
}
