using Ricimi;
using UnityEngine;
using UnityEngine.UI;

public class CustomItem : MonoBehaviour
{
    [SerializeField] private Button itemButton;
    [SerializeField] private Image iconImage; // アイコン画像

    private PartData currentPart;
    private DescriptionPopup descriptionPopup; // ポップアップの参照

    public void Setup(PartData part, DescriptionPopup popup)
    {
        currentPart = part;
        descriptionPopup = popup;
        if (iconImage != null)
        {
            iconImage.sprite = part.partIconImage; // アイコン画像を設定
            iconImage.preserveAspect = true; // アスペクト比を保持する
        }
        if (itemButton != null)
        {
            itemButton.onClick.RemoveAllListeners(); // 既存のリスナーを削除
            itemButton.onClick.AddListener(() => OnItemClick()); // ボタンにクリックイベントを追加
        }
    }
    private void OnItemClick()
    {
        descriptionPopup.Show(currentPart); // ポップアップを表示
    }
}
