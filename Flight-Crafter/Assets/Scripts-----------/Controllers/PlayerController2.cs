using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System;
using System.Collections;
using Unity.Mathematics;

public class PlayerController2 : MonoBehaviour
/*
    各パーツのステータス
    bodyData
        重量（ぶつかった時の影響）
        空気抵抗（スピード減衰率）
        体力（HP）
    rocketData
        重量（ぶつかった時の影響）
        噴射力（スピード上昇率）
        噴射時間（噴射できる時間）
    tireData
        重量（ぶつかった時の影響）
        空気抵抗（スピード減衰率）
        地上加速（地上でのスピード上昇率）
    wingData
        重量（ぶつかった時の影響）
        空気抵抗（スピード減衰率）
        空中コントロール（空中での回転力）
*/
{
    // パーツのプレハブーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public GameObject rocketThrustPrefab; // エフェクトのプレハブ
    public GameObject dieEffectPrefab; // 死亡エフェクトのプレハブ
    public Vector2 effectPosition;
    private GameObject currentRocketThrustInstance;
    private GameObject currentDieEffectInstance;

    // 各パーツデータを保持するクラスへの参照ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private BodyData bodyData; // 本体のデータ
    private RocketData rocketData; // ロケットのデータ
    private TireData tireData; // タイヤのデータ
    private WingData wingData; // 翼のデータ

    // パーツごとの合計値を計算・保持するための変数ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private float total_Weight; // 総重量
    private float total_AirResistance; //総空気抵抗
    private float total_Health; // 総体力
    private float total_JetThrust; // 総空中加速度
    private float total_Torque; // 総地上加速度
    private float total_AirControl; //空中コントロール
    private float total_RocketTime; // ロケットの合計噴射時間

    // プレイヤーゲッター
    public float TotalHealth => total_Health; // 総体力のゲッター
    public float TotalRocketTime => total_RocketTime; // ロケットの合計噴射時間のゲッター

    //イベント
    public event Action<float> OnHealthChanged;
    public event Action<float> OnRocketTimeChanged;

    // プレイヤーの制御に必要なパラメータ
    [SerializeField] private Rigidbody2D rb; // プレイヤーのRigidbody2D
    [SerializeField] private float playerAngle;
    private bool isRocketKeyDown = false;
    private float crrentRocketTime; //現在のロケット噴射時間
    private bool finishRocketTime = true; //ロケットが使えるかどうか
    [SerializeField] private LayerMask groundLayer; //GroundCheckのBoxCollider2D
    public BoxCollider2D groundCheckCollider; //GroundCheckのBoxCollider2D
    public bool groundCheck; //Groundについているかどうか
    private Vector2 rightDirection;// プレイヤーの回転に基づいた右方向

    //水に入ったとき
    [SerializeField] private LayerMask waterLayer; // Waterレイヤーを指定
    [SerializeField] private BoxCollider2D playerCollider; // プレイヤーのCapsuleCollider2D
    private bool inWater; // 水中にいるかどうか
    float MovY = 0;

    private float invincibleTime = 1.0f; // 無敵時間
    private float blinkInterval = 0.1f; // 点滅間隔
    private bool isInvincible = false; // 無敵状態かどうか
    private bool rocketPermanentlyDisabled = false;
    private SpriteRenderer[] spriteRenderers; // スプライトレンダラー

    //死んだかどうか
    private bool isDead = false;

    //クリアしたかどうか
    [SerializeField] private float clearX = 7000f; // ゴール地点X
    [SerializeField] private float clearSpeedThreshold = 1f; // 停止とみなす速度
    [SerializeField] private LayerMask runwayLayer; // 滑走路のレイヤー
    private bool isClear = false;

    void Awake()
    {
        Application.targetFrameRate = 60; // フレームレートを60に固定

        //プレイヤーのパーツデータを取得
        GetPartsData();

        // 初期ステータスを計算
        UpdateStats();
    }

    void Start()//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    {
        // Rigidbody2D を取得
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(WaitAndInitSprites());
    }
    private IEnumerator WaitAndInitSprites()
    {
        yield return null; // 1フレーム待つ

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }
    void Update() //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    {
        if (!GameManager.Instance.isClearInGameTutorial) return;
        // ゲームオーバー中なら一切の入力処理を無視
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        MovY = Input.GetAxis("Vertical");

        // プレイヤーのローカルZ回転を取得
        playerAngle = transform.localRotation.eulerAngles.z;

        //ブーストメソッド
        OnSpaceClick();

        //地面に接触しているかどうかを確認
        GroundCheck();

        //地上で加速するメソッド
        GroundAddForce();

        // プレイヤーの回転に基づいた右方向を計算
        rightDirection = new Vector2(Mathf.Cos(playerAngle * Mathf.Deg2Rad), Mathf.Sin(playerAngle * Mathf.Deg2Rad));

        //水の中にいるか判定
        InWater();

        CheckGameOver();

        CheckClear();
    }

    void FixedUpdate()
    {
        // ゲームオーバー中なら一切の入力処理を無視
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        // プレイヤーの向きを制御する
        PlayerAngle();

        RotateToMoveDirectionWithControl();
    }

    // プレイヤーのパーツ性能を集計//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void UpdateStats()
    {
        total_Weight = 0f;
        total_Torque = 0f;
        total_JetThrust = 0f;
        total_RocketTime = 0f;
        total_AirResistance = 0f;
        total_AirControl = 0f;
        total_Health = 0f;


        if (bodyData != null)
        {
            total_Weight += bodyData.weight.value;
            total_AirResistance += bodyData.airResistance.value;
            total_Health += bodyData.hp.value;
        }
        if (wingData != null)
        {
            total_Weight += wingData.weight.value;
            total_AirResistance += wingData.airResistance.value;
            total_AirControl += wingData.airControl.value;
        }
        if (tireData != null)
        {
            total_Weight += tireData.weight.value;
            total_AirResistance += tireData.airResistance.value;
            total_Torque += tireData.torque.value;
        }
        if (rocketData != null)
        {
            total_Weight += rocketData.weight.value;
            total_JetThrust += rocketData.jetThrust.value;
            total_RocketTime += rocketData.jetTime.value;
        }

        Debug.Log(
            $"総重量     :{total_Weight},\n " +
            $"総空気抵抗  :{total_AirResistance}, \n " +
            $"体力       :{total_Health}, \n " +
            $"コントロール:{total_AirControl}, \n " +
            $"地上加速    :{total_Torque}, \n " +
            $"ブースト    :{total_JetThrust}, \n " +
            $"噴射時間    :{total_RocketTime}"
            );

        rb.linearDamping = total_AirResistance; //空気抵抗。下を向くほどその方向に加速するように。
        rb.mass = total_Weight;
    }

    //プレイヤーの方向を変えるメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void PlayerAngle()
    {
        if (isDead) return; // ゲームオーバー中なら何もしない
        if (groundCheck != false) return;

        // 回転に対するトルクを加える（AddTorqueを使用）
        // TODO: 回転力の調整
        float torque = MovY * total_AirControl;  // MovYとtotal_AirControlで回転力を計算
        rb.AddTorque(torque);

        // 垂直方向の力を計算（進行方向が下向きのときの揚力）
        float thrustForce = Vector2.Dot(rb.linearVelocity, rb.GetRelativeVector(Vector2.down)) * 2.0f;

        // // 上方向に力を加えるためのベクトルを計算
        Vector2 relForce = Vector2.up * thrustForce;

        // // Rigidbody2Dに揚力を加える
        rb.AddForce(rb.GetRelativeVector(relForce));

        // 現在の角速度を取得
        float currentAngularVelocity = rb.angularVelocity;

        // 0.5秒で角速度を0に近づけるための速度を計算
        float timeToZero = 0.2f;  // 0.5秒で角速度を0にする
        float targetAngularVelocity = 0f;

        // 徐々に角速度を0に近づける
        if (Mathf.Abs(currentAngularVelocity) > 0.01f)  // 角速度が一定の閾値以上なら
        {
            // 現在の角速度を徐々に0に向けて減少させる
            rb.angularVelocity = Mathf.MoveTowards(currentAngularVelocity, targetAngularVelocity, Mathf.Abs(currentAngularVelocity) / timeToZero * Time.deltaTime);
        }
        else
        {
            // 角速度が十分小さくなったら完全に0に設定
            rb.angularVelocity = 0;
        }
    }

    //ロケットメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void OnSpaceClick()
    {
        if (isDead) return; // ゲームオーバー中なら何もしない
        if (rocketPermanentlyDisabled) return; // ← 追加
        if (inWater) return; // 水中ではロケットを使用できない
        if (!finishRocketTime) return; // ロケットの噴射時間が終了しているか、ロケットを使用できない

        bool isKeyHeld = Input.GetKey(KeyCode.Space);

        // 左シフトを押したときに
        if (isKeyHeld)
        {
            if (!isRocketKeyDown)
            {
                StartRocketEffect();
                isRocketKeyDown = true;
            }
            // ブーストしてる秒数をカウントして
            crrentRocketTime += Time.deltaTime;

            // もしブースト秒数が余ってれば
            if (total_RocketTime > crrentRocketTime)
            {
                // ロケット噴射時間を更新
                OnRocketTimeChanged?.Invoke(total_RocketTime - crrentRocketTime);
                // 回転に基づいて力の方向を計算
                Vector2 forceDirection = new Vector2(Mathf.Cos(playerAngle * Mathf.Deg2Rad), Mathf.Sin(playerAngle * Mathf.Deg2Rad));
                // プレイヤーの正面方向に力を加える
                rb.AddForce(forceDirection * total_JetThrust, ForceMode2D.Force);
            }
            else
            {
                //ロケット噴射時間が終了
                finishRocketTime = false;
                //エフェクト停止
                StopRocketEffect();
                isRocketKeyDown = false;
            }
        }
        else
        {
            if (isRocketKeyDown)
            {
                StopRocketEffect();
                isRocketKeyDown = false;
            }
        }
    }

    // ロケットのエフェクトを開始するメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private void StartRocketEffect()
    {
        currentRocketThrustInstance = Instantiate(rocketThrustPrefab, transform);
        currentRocketThrustInstance.transform.localPosition = effectPosition; // 背面に配置
    }
    // ロケットのエフェクトを停止するメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private void StopRocketEffect()
    {
        if (currentRocketThrustInstance == null) return;

        ParticleSystem ps = currentRocketThrustInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }


    //地面に接触しているかどうかを確認するメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private void GroundCheck()
    {
        if (groundCheckCollider == null) return;
        // 地面に接触しているかどうかを確認
        groundCheck = groundCheckCollider.IsTouchingLayers(groundLayer);
    }

    //車輪メソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private void GroundAddForce()
    {
        // ゲームオーバー中なら一切の入力処理を無視
        if (isDead) return;
        if (!groundCheck) return;
        // 水平方向の入力取得（Aキー: -1, Dキー: 1, それ以外: 0）
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            // 入力に基づいて力を計算
            //　TODO: 加速力の調整
            Vector2 force = rightDirection * input * total_Torque;

            // 力を加える
            rb.AddForce(force, ForceMode2D.Force);
        }
    }

    private void GetPartsData()
    {
        var currentPart = PlayerData.Instance.GetAllCurrentParts();

        if (currentPart.TryGetValue(PartType.Body, out var bodyResourceName))
        {
            bodyData = Resources.Load<BodyData>($"Parts/{bodyResourceName}");
        }
        if (currentPart.TryGetValue(PartType.Rocket, out var rocketResourceName))
        {
            rocketData = Resources.Load<RocketData>($"Parts/{rocketResourceName}");
        }
        if (currentPart.TryGetValue(PartType.Tire, out var tireResourceName))
        {
            tireData = Resources.Load<TireData>($"Parts/{tireResourceName}");
        }
        if (currentPart.TryGetValue(PartType.Wing, out var wingResourceName))
        {
            wingData = Resources.Load<WingData>($"Parts/{wingResourceName}");
        }
    }

    // 水中にいるかどうかを判定するメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private void InWater()
    {
        // 水中にいるか判定
        bool wasInWater = inWater;
        inWater = playerCollider.IsTouchingLayers(waterLayer);

        if (inWater)
        {
            //浮力を再現
            rb.AddForce(Vector2.up * 30.0f, ForceMode2D.Force);

            //水中抵抗を再現
            rb.linearDamping = 1.0f;

            // 水に入った瞬間だけ呼ぶ
            if (!wasInWater)
            {
                StopRocketEffect();
                rocketPermanentlyDisabled = true; // ← 永久封印
            }
        }
        else
        {
            //通常抵抗値に戻す
            rb.linearDamping = total_AirResistance;
        }
    }
    // ゲームオーバーのチェックメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    private void CheckGameOver()
    {
        if (transform.position.x >= 260f && rb.linearVelocity.magnitude < 1f && inWater)
        {
            GameManager.Instance.GameOver();
            rb.linearVelocity = Vector2.zero; // プレイヤーの速度をゼロにする
            rb.bodyType = RigidbodyType2D.Kinematic; // Rigidbodyをキネマティックにして動かなくする
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // 全ての動きを制限する
        }
    }

    private void CheckClear()
    {
        if (isClear) return;

        bool reachedGoal = transform.position.x >= clearX;
        bool isStopped = rb.linearVelocity.magnitude < clearSpeedThreshold;
        if (reachedGoal && isStopped && groundCheck)
        {
            isClear = true;

            rb.linearVelocity = Vector2.zero; // プレイヤーの速度をゼロにする
            rb.bodyType = RigidbodyType2D.Kinematic; // Rigidbodyをキネマティックにして動かなくする
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // 全ての動きを制限する

            GameManager.Instance.GameClear();
        }
    }

    // プレイヤーの体力を取得するメソッド
    public void TakeDamage(float damage)
    {
        // 無敵状態ならダメージを無視
        if (isInvincible) return;

        // ダメージを受けたときの処理
        Debug.Log("プレイヤーがダメージを受けました: " + damage);
        if (total_Health > 0)
        {
            total_Health -= damage;
            total_Health = Mathf.Max(total_Health, 0); // 体力が0未満にならないようにする

            // 体力が変化したことを通知
            OnHealthChanged?.Invoke(total_Health);

            // 体力が0以下になったらプレイヤーを死亡させる
            if (total_Health <= 0)
            {
                Die();
            }
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    // プレイヤーが死亡したときの処理
    private void Die()
    {
        //プレイヤーを操作できなくする
        isDead = true;
        StopRocketEffect();
        currentDieEffectInstance = Instantiate(dieEffectPrefab, transform);
        currentDieEffectInstance.transform.localPosition = effectPosition; // 背面に配置
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < invincibleTime)
        {
            SetRenderersVisible(false);
            yield return new WaitForSeconds(blinkInterval);
            SetRenderersVisible(true);
            yield return new WaitForSeconds(blinkInterval);

            timer += blinkInterval * 2;
        }

        isInvincible = false;
    }

    private void SetRenderersVisible(bool visible)
    {
        foreach (var sr in spriteRenderers)
        {
            sr.enabled = visible;
        }
    }
    
    private void RotateToMoveDirectionWithControl()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude < 0.01f)
            return;

        float moveAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        float currentAngle = rb.rotation;

        float angleDifference = Mathf.DeltaAngle(currentAngle, moveAngle);
        float speedFactor = velocity.magnitude * 0.005f;
        float waterMultiplier = inWater ? 0.01f : 2.0f; // 水中では回転を遅くする
        
        float correctionTorque = angleDifference * speedFactor * waterMultiplier;  // ← 補正の強さ（0.1f を好みに調整）

        rb.AddTorque(correctionTorque);
    }
}
