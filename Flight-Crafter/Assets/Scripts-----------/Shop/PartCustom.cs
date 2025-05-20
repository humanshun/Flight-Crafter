using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartCustom : MonoBehaviour
{
    [SerializeField] private Button bodyPopupButton;
    [SerializeField] private Button rocketPopupButton;
    [SerializeField] private Button tirePopupButton;
    [SerializeField] private Button wingPopupButton;
    [SerializeField] Transform contentTransform;
    [SerializeField] private GameObject partCostom;
    [SerializeField] private DescriptionPopup descriptionPopup;
    [SerializeField] private ShopData shopData;
    [SerializeField] private GameObject itemPrefab;


    void Start()
    {
        bodyPopupButton.onClick.AddListener(() => OnButtonClick(bodyPopupButton, PartType.Body));
        rocketPopupButton.onClick.AddListener(() => OnButtonClick(rocketPopupButton, PartType.Rocket));
        tirePopupButton.onClick.AddListener(() => OnButtonClick(tirePopupButton, PartType.Tire));
        wingPopupButton.onClick.AddListener(() => OnButtonClick(wingPopupButton, PartType.Wing));

        descriptionPopup.gameObject.SetActive(false); // 初期状態で非表示
        partCostom.SetActive(false); // 初期状態で非表示
    }
    private void OnButtonClick(Button selectedButton, PartType selectedType)
    {
        partCostom.SetActive(true); // Contentをアクティブにする
        // Contentを全クリア
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        List<PartData> parts = null;

        switch (selectedType)
        {
            case PartType.Body:
                parts = shopData.typeBody.bodyParts;
                break;
            case PartType.Rocket:
                parts = shopData.typeRocket.rocketParts;
                break;
            case PartType.Tire:
                parts = shopData.typeTire.tireParts;
                break;
            case PartType.Wing:
                parts = shopData.typeWing.wingParts;
                break;
        }

        foreach (PartData part in parts)
        {
            // if (part.isPurchased) // 購入済みのパーツのみ処理
            // {
            //     GameObject item = Instantiate(itemPrefab, contentTransform);
            //     CustomItem setupItem = item.GetComponent<CustomItem>();
            //     if (setupItem != null)
            //     {
            //         setupItem.Setup(part, descriptionPopup); // データを渡してセットアップ
            //     }
            // }
            if (PlayerData.Instance.IsPartPurchased(part.partName))
            {
                GameObject item = Instantiate(itemPrefab, contentTransform);
                CustomItem setupItem = item.GetComponent<CustomItem>();
                if (setupItem != null)
                {
                    setupItem.Setup(part, descriptionPopup); // データを渡してセットアップ
                }
            }
        }
    }
}
