using UnityEngine;
using UnityEngine.UI;

public class ShopCanvasManager : MonoBehaviour
{
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject partShopPopup;
    [SerializeField] private Button shopCloseButton;
    [SerializeField] private GameObject partCustomPopup;
    [SerializeField] private Button customCloseButton;

    [SerializeField] private Button bodyButton;
    [SerializeField] private Button rocketButton;
    [SerializeField] private Button tireButton;
    [SerializeField] private Button wingButton;

    [SerializeField] private GameObject desctiptionPopupPrefab;
    [SerializeField] private GameObject coinText;

    private void Start()
    {
        // ボタンにイベントリスナーを追加
        shopButton.onClick.AddListener(OnShopButtonClicked);

        shopCloseButton.onClick.AddListener(OnShopCloseButtonClicked);
        customCloseButton.onClick.AddListener(OnCustomCloseButtonClicked);

        bodyButton.onClick.AddListener(CustomButtonOnClick);
        rocketButton.onClick.AddListener(CustomButtonOnClick);
        tireButton.onClick.AddListener(CustomButtonOnClick);
        wingButton.onClick.AddListener(CustomButtonOnClick);

    }

    private void OnShopButtonClicked()
    {
        partShopPopup.SetActive(true);
        partCustomPopup.SetActive(false);
        shopButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        coinText.SetActive(true);
    }

    private void OnShopCloseButtonClicked()
    {
        partShopPopup.SetActive(false);
        partCustomPopup.SetActive(true);
        shopButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        coinText.SetActive(true);
    }

    private void OnCustomCloseButtonClicked()
    {
        shopButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        desctiptionPopupPrefab.SetActive(false);
        coinText.SetActive(true);
    }

    private void CustomButtonOnClick()
    {
        shopButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        desctiptionPopupPrefab.SetActive(false);
        coinText.SetActive(false);
    }
}
