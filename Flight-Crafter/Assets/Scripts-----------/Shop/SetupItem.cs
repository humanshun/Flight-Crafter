using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button costButton;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite active;
    [SerializeField] private Sprite inactive;
    private PartData currentPart;


    void Start()
    {
        // ボタンのクリックイベントを登録
        if (costButton != null)
        {
            costButton.onClick.AddListener(OnCostButtonClick);
        }
    }

    public void Setup(PartData part)
    {
        currentPart = part;
        if (nameText != null) nameText.text = part.partName;
        if (costText != null) costText.text = $"¥{part.partCost}";
        if (descriptionText != null) descriptionText.text = part.partDescription;
        if (iconImage != null) iconImage.sprite = part.partIconImage;
        if (iconImage != null) iconImage.preserveAspect = true; // アスペクト比を保持する

        if (costButton != null)
        {
            costButton.image.sprite = part.isPurchased ? inactive : active; // 購入済みなら非アクティブスプライトにする
            costButton.interactable = !part.isPurchased; // 購入済みならボタンを無効にする
        }
    }

    private void OnCostButtonClick()
    {
        // 購入処理を実行
        if (PlayerData.Instance.TryBuyPart(currentPart.partCost))
        {
            currentPart.isPurchased = true; // 購入フラグを立てる
            costButton.image.sprite = inactive; // ボタンを非アクティブスプライトにする
            costButton.interactable = false; // ボタンを無効にする
            Debug.Log("Item purchased successfully!");
            // 購入成功の処理を追加することができます
        }
        else
        {
            
            Debug.Log("Not enough coins to buy this item.");
            // 購入失敗の処理を追加することができます
        }
    }
}
