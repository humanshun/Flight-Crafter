using UnityEngine;

public class LookingBird : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject headRight;
    [SerializeField] private GameObject bodyRight;
    [SerializeField] private GameObject footRight;
    [SerializeField] private GameObject headLeft;
    [SerializeField] private GameObject bodyLeft;
    [SerializeField] private GameObject footLeft;
    [SerializeField] private Transform birdTransform;
    private bool isFacingRight = true;
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
        birdTransform = spawnedPlayer.transform;
        playerSprite.enabled = false;
        SetRight(true);  // 初期状態を右向きにする
        bool isLeft = birdTransform.position.x < transform.position.x;
        // 現在アクティブなheadを取得
        GameObject activeHead = isLeft ? headLeft : headRight;

        // 方向ベクトル → 回転角度 → 反映
        Vector3 direction = (birdTransform.position - activeHead.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 上向き基準の補正（スプライトが上を向いている想定）
        activeHead.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }

    void Start()
    {
        playerSprite.enabled = false;
        SetRight(true);  // 初期状態を右向きにする
        bool isLeft = birdTransform.position.x < transform.position.x;
        // 現在アクティブなheadを取得
        GameObject activeHead = isLeft ? headLeft : headRight;

        // 方向ベクトル → 回転角度 → 反映
        Vector3 direction = (birdTransform.position - activeHead.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 上向き基準の補正（スプライトが上を向いている想定）
        activeHead.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }

    void Update()
    {
        bool isLeft = birdTransform.position.x < transform.position.x;

        if (isFacingRight == isLeft) // 方向変化したときだけ切り替え
        {
            isFacingRight = !isLeft;
            SetRight(!isLeft);
        }

        // 現在アクティブなheadを取得
        GameObject activeHead = isLeft ? headLeft : headRight;

        // 方向ベクトル → 回転角度 → 反映
        Vector3 direction = (birdTransform.position - activeHead.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 上向き基準の補正（スプライトが上を向いている想定）
        activeHead.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }

    void SetRight(bool isRight)
    {
        headRight.SetActive(isRight);
        bodyRight.SetActive(isRight);
        footRight.SetActive(isRight);

        headLeft.SetActive(!isRight);
        bodyLeft.SetActive(!isRight);
        footLeft.SetActive(!isRight);
    }
    void Set()
    {
        headRight.SetActive(false);
        bodyRight.SetActive(false);
        footRight.SetActive(false);

        headLeft.SetActive(false);
        bodyLeft.SetActive(false);
        footLeft.SetActive(false);
    }

    public void BackAnim()
    {
        Set();
        playerSprite.enabled = true;
        this.enabled = false;
    }
}
