using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Security;

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
        tutorialText.text = "家の倉庫へようこそ！\n\nここでは、あなたの家の倉庫を管理できます。\n\nまずは、倉庫の中身を確認しましょう。";
    }
    private void OnEnable()
    {
        nextButton.onClick.AddListener(NextStep);
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
        nextButton.onClick.RemoveListener(NextStep);

    }

    public void NextStep()
    {
        switch (currentStep)
        {
            case TutorialStep.Step1:
                tutorialText.text = "家の倉庫へようこそ！\n\nここでは、あなたの家の倉庫を管理できます。\n\nまずは、倉庫の中身を確認しましょう。";
                tutorialPanel.SetActive(true);
                currentStep++;
                break;
            case TutorialStep.Step2:
                tutorialText.text = "倉庫の中身を確認するには、倉庫のアイコンをクリックしてください。\n\n倉庫の中身が表示されます。";
                bodyCustomButton.interactable = true;
                rocketCustomButton.interactable = true;
                tireCustomButton.interactable = true;
                wingCustomButton.interactable = true;
                currentStep++;
                break;
            case TutorialStep.Step3:
                tutorialText.text = "qqqqqqqqqqqqqqqqq";
                currentStep++;
                break;
        }
    }

}
