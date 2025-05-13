using TMPro;
using UnityEngine;

public class DescriptionPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot; // ポップアップの本体
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText; // 任意

    public void Show(PartData part)
    {
        popupRoot.SetActive(true);
        nameText.text = part.partName;

        // 必要に応じてステータス情報も表示（例）
        statsText.text = $"{part.weight.displayName}:{part.weight.value}";
    }

    public void Hide()
    {
        popupRoot.SetActive(false);
    }
}
