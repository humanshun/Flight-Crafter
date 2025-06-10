using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button playButton;

    [Header("ショップボタン")]
    [SerializeField] private Button shopButton;
    [SerializeField] private Button bodyButton;
    [SerializeField] private Button rocketButton;
    [SerializeField] private Button tireButton;
    [SerializeField] private Button wingButton;
    [SerializeField] private Button closeButton;

    [Header("倉庫ボタン")]

    [SerializeField] private Button bodyCustomButton;
    [SerializeField] private Button rocketCustomButton;
    [SerializeField] private Button tireCustomButton;
    [SerializeField] private Button wingCustomButton;
    [SerializeField] private Button customCloseButton;
    [SerializeField] private Button setButton;

    [Header("矢印")]
    [SerializeField] private GameObject arrowUp;
    [SerializeField] private GameObject arrowDown;
    [SerializeField] private GameObject arrowLeft;
    [SerializeField] private GameObject arrowRight;
    private GameObject arrowPrefab;

    private TutorialStep currentStep = TutorialStep.Step1;

    enum TutorialStep
    {
        Step1,
        Step2,
        Step3,
        Step4,
        Step5,
        Step6,
        Step7,
        Step8,
        Step9,
        Step10,
    }
    private void Start()
    {
        tutorialPanel.SetActive(true);
        nextButton.onClick.AddListener(NextStep);
        bodyCustomButton.onClick.AddListener(OnBodyCustomClicked);
        setButton.onClick.AddListener(OnSetButtonClicked);
        NextStep();
    }
    private void OnEnable()
    {
        PlayerData.OnAnyPartEquipped += CheckAllPartsEquipped;


        playButton.interactable = false;
        shopButton.interactable = false;
        bodyButton.interactable = false;
        rocketButton.interactable = false;
        tireButton.interactable = false;
        wingButton.interactable = false;
        closeButton.interactable = false;
        bodyCustomButton.interactable = false;
        rocketCustomButton.interactable = false;
        tireCustomButton.interactable = false;
        wingCustomButton.interactable = false;
        customCloseButton.interactable = false;
        setButton.interactable = false;
    }
    private void OnDisable()
    {
        PlayerData.OnAnyPartEquipped -= CheckAllPartsEquipped;
        nextButton.onClick.RemoveListener(NextStep);

    }

    public void NextStep()
    {
        switch (currentStep)
        {
            case TutorialStep.Step1:
                tutorialText.text = "家の倉庫へようこそ！\n\nここでは、あなたの機体を管理できます。\n\nまずは、ボディ倉庫の中身を確認しましょう。";
                tutorialPanel.SetActive(true);
                currentStep++;
                break;
            case TutorialStep.Step2:
                tutorialText.text = "ボディ倉庫の中身を確認するには、倉庫のアイコンをクリックしてください。\n\nボディ倉庫の中身が表示されます。";
                if (arrowPrefab == null)
                {
                    arrowPrefab = Instantiate(arrowDown, new Vector3(290, 340, 0), Quaternion.identity, tutorialPanel.transform);
                    arrowPrefab.transform
                        .DOMoveY(arrowPrefab.transform.position.y - 20f, 0.5f)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                }
                bodyCustomButton.interactable = true;
                break;
            case TutorialStep.Step3:
                tutorialText.text = "次に、パーツを選択して装備しよう!\n\nボディを選択して、装備ボタンをクリックしてください。";
                if (arrowPrefab != null)
                {
                    Destroy(arrowPrefab.gameObject);
                    arrowPrefab = Instantiate(arrowRight, new Vector3(1050, 900, 0), Quaternion.identity, tutorialPanel.transform);
                    arrowPrefab.transform
                        .DOMoveX(arrowPrefab.transform.position.x + 20f, 0.5f)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                }
                bodyCustomButton.interactable = false;
                setButton.interactable = true;
                break;
            case TutorialStep.Step4:
                tutorialText.text = "装備が完了しました!\n\nでは、他のパーツを装備してみましょう。";
                if (arrowPrefab != null)
                {
                    Destroy(arrowPrefab.gameObject);
                }
                customCloseButton.interactable = true;
                rocketCustomButton.interactable = true;
                tireCustomButton.interactable = true;
                wingCustomButton.interactable = true;
                break;
            case TutorialStep.Step5:
                tutorialText.text = "すべてのパーツを装備しました!\n\nでは、大空へ飛び立ちましょう!";
                setButton.interactable = false;
                bodyCustomButton.interactable = false;
                rocketCustomButton.interactable = false;
                tireCustomButton.interactable = false;
                wingCustomButton.interactable = false;
                break;
            case TutorialStep.Step6:
                tutorialText.text = "すべてのパーツを装備しました!\n\nでは、大空へ飛び立ちましょう!";
                
                break;
        }
    }

    void OnBodyCustomClicked()
    {
        if (currentStep == TutorialStep.Step2)
        {
            currentStep++;
            NextStep();
        }
    }

    void OnSetButtonClicked()
    {
        if (currentStep == TutorialStep.Step3)
        {
            currentStep++;
            NextStep();
        }
    }
    private void CheckAllPartsEquipped()
    {
        if (currentStep == TutorialStep.Step4 &&
            PlayerData.Instance.HasAllRequiredPartsEquipped())
        {
            currentStep++;
            NextStep();
        }
    }
}
