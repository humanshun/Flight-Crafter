using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialInGame : MonoBehaviour
{
    [SerializeField] private Button[] tutorialPanels;
    [SerializeField] private GameObject caretDownImage;
    private int currentStep = 0;

    void Start()
    {
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

    private void NextStep()
    {
        tutorialPanels[currentStep].gameObject.SetActive(false);
        caretDownImage.SetActive(false);
        currentStep++;

        if (currentStep < tutorialPanels.Length)
        {
            tutorialPanels[currentStep].gameObject.SetActive(true);
            caretDownImage.SetActive(true);
        }
        else
        {
            Debug.Log("チュートリアル終了");
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
