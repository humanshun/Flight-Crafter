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
        // 購入ボタンにクリックイベント登録
        if (costButton != null)
        {
            costButton.onClick.AddListener(OnCostButtonClick);
        }
    }

    public void Setup(PartData part)
    {
        currentPart = part;

        // UIに情報を反映
        nameText.text = part.partName;
        costText.text = $"¥{part.partCost.value}";
        descriptionText.text = part.partDescription;

        if (iconImage != null)
        {
            iconImage.sprite = part.partIconImage;
            iconImage.preserveAspect = true;
        }

        // 購入状態に応じてボタンの状態を更新
        UpdateButtonVisual();
    }

    private void UpdateButtonVisual()
    {
        if (costButton != null)
        {
            bool isPurchased = PlayerData.Instance.IsPartPurchased(currentPart.partName);
            costButton.image.sprite = isPurchased ? inactive : active;
            costButton.interactable = !isPurchased;
        }
    }

    private void OnCostButtonClick()
    {
        // コインを消費して購入を試みる
        if (PlayerData.Instance.TryBuyPart(currentPart, currentPart.partCost.value))
        {
            PlayerData.Instance.SavePurchasedPart(currentPart); // JSONに保存
            UpdateButtonVisual(); // 最新状態をUIに反映

            Debug.Log("アイテムの購入に成功しました！");
        }
        else
        {
            Debug.Log("このアイテムを買うにはコインが足りません。");
        }
    }
}
