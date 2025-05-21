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
    [SerializeField] private CurrentPartPopup bodyCurrentPartPopup;
    [SerializeField] private CurrentPartPopup rocketCurrentPartPopup;
    [SerializeField] private CurrentPartPopup tireCurrentPartPopup;
    [SerializeField] private CurrentPartPopup wingCurrentPartPopup;
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

        // 装備中パーツをUIに反映
        SetupEquippedParts();
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
        CurrentPartPopup currentPartPopup = null;

        switch (selectedType)
        {
            case PartType.Body:
                parts = shopData.typeBody.bodyParts;
                currentPartPopup = bodyCurrentPartPopup;
                break;
            case PartType.Rocket:
                parts = shopData.typeRocket.rocketParts;
                currentPartPopup = rocketCurrentPartPopup;
                break;
            case PartType.Tire:
                parts = shopData.typeTire.tireParts;
                currentPartPopup = tireCurrentPartPopup;
                break;
            case PartType.Wing:
                parts = shopData.typeWing.wingParts;
                currentPartPopup = wingCurrentPartPopup;
                break;
        }

        foreach (PartData part in parts)
        {
            if (PlayerData.Instance.IsPartPurchased(part.partName))
            {
                GameObject item = Instantiate(itemPrefab, contentTransform);
                CustomItem setupItem = item.GetComponent<CustomItem>();
                if (setupItem != null)
                {
                    setupItem.Setup(part, descriptionPopup, currentPartPopup); // データを渡してセットアップ
                }
            }
        }
    }
    private void SetupEquippedParts()
    {
        Dictionary<PartType, string> parts = PlayerData.Instance.GetAllCurrentParts();

        foreach (var kvp in parts)
        {
            PartType type = kvp.Key;
            string resourceName = kvp.Value;
            PartData part = Resources.Load<PartData>($"Parts/{resourceName}");
            if (part == null)
            {
                Debug.LogWarning($"{type} の {resourceName} が見つかりませんでした。");
                continue;
            }

            switch (type)
            {
                case PartType.Body:
                    bodyCurrentPartPopup.Setup(part);
                    break;
                case PartType.Rocket:
                    rocketCurrentPartPopup.Setup(part);
                    break;
                case PartType.Tire:
                    tireCurrentPartPopup.Setup(part);
                    break;
                case PartType.Wing:
                    wingCurrentPartPopup.Setup(part);
                    break;
            }
        }
    }

}
