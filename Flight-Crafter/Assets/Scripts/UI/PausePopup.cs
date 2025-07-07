using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePopup : MonoBehaviour
{
    [SerializeField] private GameObject pausePopup;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private Button quitButton;
    private GameObject popupInstance;
    private void Start()
    {
        popupInstance = pausePopup;
        GameManager.Instance.pausePopup = this;
        popupInstance.gameObject.SetActive(false);

        resumeButton.onClick.AddListener(OnClickResume);
        restartButton.onClick.AddListener(OnClickRestart);
        titleButton.onClick.AddListener(OnClickTitle);
        quitButton.onClick.AddListener(OnClickQuit);
    }
    public bool IsShowing()
    {
        return popupInstance != null && popupInstance.activeSelf;
    }
    public void Show()
    {
        if (popupInstance != null && !popupInstance.activeSelf)
        {
            popupInstance.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Hide()
    {
        if (popupInstance != null && popupInstance.activeSelf)
        {
            popupInstance.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    private void OnClickResume()
    {
        AudioManager.Instance.PlaySFX("SE_ButtonLow");
        Hide(); // 時間再開
    }

    private async void OnClickRestart()
    {
        Time.timeScale = 1f; // 時間再開
        AudioManager.Instance.PlaySFX("SE_ButtonLow");
        await SceneChanger.Instance.ChangeScene(SceneManager.GetActiveScene().name, 0.5f, 0.5f);
    }

    private async void OnClickTitle()
    {
        Time.timeScale = 1f; // 時間再開
        AudioManager.Instance.PlaySFX("SE_ButtonLow");
        await SceneChanger.Instance.ChangeScene("Title", 0.5f, 0.5f);
    }

    private void OnClickQuit()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX("SE_ButtonLow");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ再生を止める
#else
        Application.Quit(); // ビルド版ではアプリ終了
#endif
    }
}
