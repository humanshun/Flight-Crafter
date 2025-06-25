using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePopup : MonoBehaviour
{
    [SerializeField] private GameObject pausePopup;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    private GameObject popupInstance;
    private void Start()
    {
        popupInstance = pausePopup;
        GameManager.Instance.pausePopup = this;
        popupInstance.gameObject.SetActive(false);

        resumeButton.onClick.AddListener(OnClickResume);
        restartButton.onClick.AddListener(OnClickRestart);
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
        Hide(); // 時間再開
    }

    private void OnClickRestart()
    {
        Time.timeScale = 1f; // 時間再開
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnClickQuit()
    {
        Time.timeScale = 1f;
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ再生を止める
    #else
        Application.Quit(); // ビルド版ではアプリ終了
    #endif
    }
}
