using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button closeButton2;

    private void Start()
    {
        Time.timeScale = 0; // ゲームの時間を停止
        // ボタンにイベントリスナーを追加
        closeButton.onClick.AddListener(OnCloseButtonClicked);
        closeButton2.onClick.AddListener(OnCloseButtonClicked);
    }
    private void OnCloseButtonClicked()
    {
        Time.timeScale = 1; // ゲームの時間を再開
        AudioManager.Instance.PlaySFX("SE_Close");
        gameObject.SetActive(false); // ポップアップを非アクティブにする
    }
}
