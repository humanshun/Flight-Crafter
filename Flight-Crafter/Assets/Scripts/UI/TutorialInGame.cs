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
    [SerializeField] private Transform playerTransform;            // プレイヤーのTransform
    private bool initialized = false;
    [SerializeField] private float[] stepTriggers;        // 各ステップのX座標トリガー
    
    private bool[] stepActive;                            // ステップごとのアクティブ状態
    private bool tutorial = false;
    private int currentStep = 0;
    void OnEnable()
    {
        GameManager.OnInGamePlayerSpawned += OnPlayerSpawned;
    }

    void OnDisable()
    {
        GameManager.OnInGamePlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(CustomPlayer spawnedPlayer)
    {
        playerTransform = spawnedPlayer.transform;
        initialized = true;
    }

    void Start()
    {
        int completed = PlayerPrefs.GetInt("InGameTutorialCompleted", 0);
        Debug.Log($"チュートリアル完了フラグ: {completed}");

        // チュートリアル済みならスキップ
        if (PlayerPrefs.GetInt("InGameTutorialCompleted", 0) == 1)
        {
            tutorial = true;
            this.enabled = false;
            Time.timeScale = 1f; // ★ここを追加！
            GameManager.Instance.isTutorial = false;
            caretDownImage.SetActive(false);
        }

        GameManager.Instance.TutorialInGamePopup(tutorialPopup);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TutorialInGamePopup(this);
            Scene currentScene = SceneManager.GetActiveScene();
            GameManager.Instance.TutorialShow(currentScene);
        }

        stepActive = new bool[tutorialPanels.Length];

        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            if (tutorialPanels[i] == null)
            {
                Debug.LogError($"チュートリアルパネル{i}が設定されていません。");
                continue;
            }
            tutorialPanels[i].onClick.AddListener(DeactivateCurrentStep); // ボタンクリックで非表示
            tutorialPanels[i].gameObject.SetActive(false);
            stepActive[i] = false;
        }

        if (tutorialPanels.Length > 0  && completed == 0)
        {
            tutorialPanels[0].gameObject.SetActive(true);
            stepActive[0] = true;
            Time.timeScale = 0f;
            GameManager.Instance.isTutorial = true;
        }

        StartCaretAnimation();
        caretDownImage.SetActive(true);
    }

    void Update()
    {
        if (tutorial || !initialized) return;

        // プレイヤーの座標で次ステップに進む
        if (currentStep < stepTriggers.Length &&
            playerTransform.position.x >= stepTriggers[currentStep] &&
            !stepActive[currentStep])
        {
            NextStep();
            Time.timeScale = 0f;
            GameManager.Instance.isTutorial = true;
        }

        // Enterキーでも進める
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DeactivateCurrentStep();
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

        // 今のパネルを非表示にして次へ
        if (currentStep < tutorialPanels.Length)
        {
            tutorialPanels[currentStep].gameObject.SetActive(false);
            caretDownImage.SetActive(false);
            stepActive[currentStep] = false;
        }

        currentStep++;

        if (currentStep == 3)
        {
            tutorialInGameCheckList.CheckList();
        }

        if (currentStep < tutorialPanels.Length)
        {
            tutorialPanels[currentStep].gameObject.SetActive(true);
            caretDownImage.SetActive(true);
            stepActive[currentStep] = true;
        }
        if (currentStep == tutorialPanels.Length - 1)
        {
            GameManager.Instance.isClearInGameTutorial = true;
            tutorial = true;
            Time.timeScale = 1f;
            GameManager.Instance.isTutorial = false;

            PlayerPrefs.SetInt("InGameTutorialCompleted", 1);
            PlayerPrefs.Save();
            Debug.Log("✅ チュートリアル完了として保存しました");
        }
    }

    private void DeactivateCurrentStep()
    {
        // 現在のステップを非表示にするだけ
        if (currentStep < tutorialPanels.Length && stepActive[currentStep])
        {
            tutorialPanels[currentStep].gameObject.SetActive(false);
            caretDownImage.SetActive(false);
            stepActive[currentStep] = false;

            Time.timeScale = 1f;
            GameManager.Instance.isTutorial = false;
        }
    }

    private void StartCaretAnimation()
    {
        float moveAmount = 20f;
        Vector3 target = Vector3.down * moveAmount;
        caretDownImage.transform.DOLocalMove(caretDownImage.transform.localPosition + target, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);
    }
}
