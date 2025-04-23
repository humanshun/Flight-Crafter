using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopPartsButton : MonoBehaviour
{
    [SerializeField] Button bodyButton;
    [SerializeField] Button rocketButton;
    [SerializeField] Button tireButton;
    [SerializeField] Button wingButton;

    [SerializeField] Sprite enabledSprite;
    [SerializeField] Sprite disabledSprite;


    public ShopData shopData;
    public GameObject itemPrefab;
    void Start()
    {
        bodyButton.onClick.AddListener(() => OnButtonClicked(bodyButton, PartType.Body));
        rocketButton.onClick.AddListener(() => OnButtonClicked(rocketButton, PartType.Rocket));
        tireButton.onClick.AddListener(() => OnButtonClicked(tireButton, PartType.Tire));
        wingButton.onClick.AddListener(() => OnButtonClicked(wingButton, PartType.Wing));

        OnButtonClicked(bodyButton, PartType.Body); // 初期状態の更新
    }

    void OnButtonClicked(Button selectedButton, PartType selectedType)
    {
        Button[] buttons = { bodyButton, rocketButton, tireButton, wingButton };

        foreach (Button btn in buttons)
        {
            bool isSelected = (btn == selectedButton);
            btn.interactable = !isSelected;

            Transform childImage = btn.transform.Find("ButtonImage");
            if (childImage != null)
            {
                Image img = childImage.GetComponent<Image>();
                if (img != null)
                {
                    img.sprite = isSelected ? enabledSprite : disabledSprite;
                }
            }
        }
        // 選ばれたカテゴリのパーツを表示
        UpdatePartListUI(selectedType);

        
    }
    void UpdatePartListUI(PartType partType)
    {
        // Contentオブジェクトを取得
        Transform contentTransform = transform.Find("Scroll View/View Port/Content");
        // Contentを全クリア
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (PartData part in shopData.shopItems)
        {
            if (part.partType == partType)
            {
                Debug.Log(part.name + " (" + part.partType + ")");
                var item = Instantiate(itemPrefab) as GameObject;
                
                // Contentオブジェクトの子として設定
                item.transform.SetParent(contentTransform.transform, false);

                Transform nameTextTransform = item.transform.Find("Text/Name Text");
                Transform descriptioTextTransform = item.transform.Find("Text/DescriptioText");
                Transform priceTextTransform = item.transform.Find("Button/Text");
                Transform iconTransform = item.transform.Find("Icon");

                TMPro.TextMeshProUGUI nameText = nameTextTransform?.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (nameText != null)
                {
                    nameText.text = part.name;
                }
                else
                {
                    Debug.LogError("nameTextのTextMeshProUGUIコンポーネントが見つかりません。");
                }

                TMPro.TextMeshProUGUI descriptioText = descriptioTextTransform?.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (descriptioText != null)
                {
                    descriptioText.text = part.partDescription;
                }
                else
                {
                    Debug.LogError("partDescriptionのTextMeshProUGUIコンポーネントが見つかりません。");
                }

                TMPro.TextMeshProUGUI priceText = priceTextTransform?.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (priceText != null)
                {
                    priceText.text = part.partCost.ToString() + "$";
                }
                else
                {
                    Debug.LogError("TextMeshProUGUIコンポーネントが見つかりません。");
                }

                Image iconImage = iconTransform?.GetComponent<Image>();
                if (iconImage != null)
                {
                    iconImage.sprite = part.partIconImage;
                    iconImage.preserveAspect = true;
                }
                else
                {
                    Debug.LogError("iconTransformのImageコンポーネントが見つかりません。");
                }
            }
        }
    }
}
