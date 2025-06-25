using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TutorialInGame : MonoBehaviour
{
    [SerializeField] private TutorialInGame tutorialPopup;
    [SerializeField] private Button[] tutorialPanels;
    [SerializeField] private GameObject caretDownImage;
    [SerializeField] private TutorialInGameCheckList tutorialInGameCheckList;
    private bool tutorial = false;
    private int currentStep = 0;

    void Start()
    {
        GameManager.Instance.TutorialInGamePopup(tutorialPopup);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TutorialInGamePopup(this);
            Scene currentScene = SceneManager.GetActiveScene();
            GameManager.Instance.TutorialShow(currentScene);
        }
        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            if (tutorialPanels[i] == null)
            {
                Debug.LogError($"チュートリアルパネル{i}が設定されていません。");
                continue;
            }
            tutorialPanels[i].onClick.AddListener(NextStep);
            tutorialPanels[i].gameObject.SetActive(false);
        }

        if (tutorialPanels.Length > 0)
        {
            tutorialPanels[0].gameObject.SetActive(true);
        }
        StartCaretAnimation();
        caretDownImage.SetActive(true);
    }
    
    void Update()
    {
        if (tutorial) return;
        if (Input.GetKeyDown(KeyCode.Return))  // Enterキーでも次へ
        {
            NextStep();
        }
    }

    private void NextStep()
    {
        if (currentStep >= tutorialPanels.Length)
        {
            Debug.Log("チュートリアルはすでに終了しています。");
            tutorial = true;
            return;
        }

        tutorialPanels[currentStep].gameObject.SetActive(false);
        caretDownImage.SetActive(false);
        currentStep++;
        if (currentStep == 3)
        {
            tutorialInGameCheckList.CheckList();
        }
        if (currentStep < tutorialPanels.Length)
        {
            tutorialPanels[currentStep].gameObject.SetActive(true);
            caretDownImage.SetActive(true);
        }
        else
        {
            GameManager.Instance.isClearInGameTutorial = true;
            tutorial = true;
        }
    }
    private void StartCaretAnimation()
    {
        float moveAmount = 20f;
        Vector3 target = Vector3.down * moveAmount;
        caretDownImage.transform.DOLocalMove(caretDownImage.transform.localPosition + target, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
