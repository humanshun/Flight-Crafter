using TMPro;
using UnityEngine;

public class DescriptionPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupRoot; // ポップアップの本体
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Transform ContentTransform;

    public void Show(PartData part)
    {
        popupRoot.SetActive(true);
        nameText.text = part.partName;

        // 必要に応じてステータス情報も表示（例）
        // statsText[0].text = $"{part.weight.displayName}:{part.weight.value}";

        //ToDo パーツのタイプに応じてStatusPrefabを複数ContentTransformにInstantiate

    }

    public void Hide()
    {
        popupRoot.SetActive(false);
    }
}
