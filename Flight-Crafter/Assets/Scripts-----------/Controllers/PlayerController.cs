using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System;

public class PlayerController : MonoBehaviour
{
    // パーツのプレハブーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public GameObject bodyPrefab;
    public GameObject rocketPrefab;
    public GameObject tirePrefab;
    public GameObject wingPrefab;

    // 各パーツデータを保持するクラスへの参照ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private BodyData bodyData; // 本体のデータ
    private RocketData rocketData; // ロケットのデータ
    private TireData tireData; // タイヤのデータ
    private WingData wingData; // 翼のデータ

    // パーツごとの合計値を計算・保持するための変数ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private float total_Weight; // 総重量
    private float total_JetThrust; // 総空中加速度
    private float total_Torque; // 総地上加速度
    private float total_Lift; // 総浮力
    private float total_AirControl; //空中コントロール
    private float total_AirRotationalControl;
    private float total_RocketTime; // ロケットの合計噴射時間
    private float total_AirResistance; //総空気抵抗

    // プレイヤーの制御に必要なパラメータ
    private Rigidbody2D rb; // プレイヤーのRigidbody2D
    [SerializeField] private float playerAngle;
    private float crrentRocketTime; //現在のロケット噴射時間
    private bool finishRocketTime = true; //ロケットが使えるかどうか
    [SerializeField] private float maxRotationAngle = 90f; // 最大回転角度（±）
    public bool groundCheck;

    void Start()
    {
        // Rigidbody2D を取得
        rb = GetComponent<Rigidbody2D>();

        // パーツデータを初期化
        InitializeParts();

        // 初期ステータスを計算
        UpdateStats();
    }
    void Update()
    {
        // プレイヤーの向きを制御する
        if (wingPrefab != null) {PlayerAngle();}

        // 左クリックの状態をチェック
        if (rocketPrefab != null) {OnLeftClick();}

        // プレイヤーのローカルZ回転を取得
        playerAngle = transform.localRotation.eulerAngles.z;

        GroundAddForce();
    }

    // パーツデータを初期化
    void InitializeParts()
    {
        if (bodyPrefab != null)
        {
            Part bodyPart = bodyPrefab.GetComponent<Part>();
            if (bodyPart != null && bodyPart.partData is BodyData body)
            {
                bodyData = body;
                Debug.Log($"Bodyデータを取得: {bodyData.weight}, {bodyData.airResistance}");
            }
        }

        if (wingPrefab != null)
        {
            Part wingPart = wingPrefab.GetComponent<Part>();
            if (wingPart != null && wingPart.partData is WingData wing)
            {
                wingData = wing;
                Debug.Log($"Wingデータを取得: {wingData.weight}, {wingData.lift}");
            }
        }

        if (tirePrefab != null)
        {
            Part tirePart = tirePrefab.GetComponent<Part>();
            if (tirePart != null && tirePart.partData is TireData tire)
            {
                tireData = tire;
                Debug.Log($"Tireデータを取得: {tireData.weight}, {tireData.torque}");
            }
        }

        if (rocketPrefab != null)
        {
            Part rocketPart = rocketPrefab.GetComponent<Part>();
            if (rocketPart != null && rocketPart.partData is RocketData rocket)
            {
                rocketData = rocket;
                Debug.Log($"Rocketデータを取得: {rocketData.weight}, {rocketData.jetThrust}, {rocketData.time}");
            }
        }
    }

    // プレイヤーのパーツ性能を集計
    public void UpdateStats()
    {
        total_Weight = 0f;
        total_Lift = 0f;
        total_Torque = 0f;
        total_JetThrust = 0f;
        total_RocketTime = 0f;
        total_AirResistance = 0f;

        if (bodyData != null)
        {
            total_Weight += bodyData.weight;
            total_AirResistance += bodyData.airResistance;
        }
        if (wingData != null)
        {
            total_Weight += wingData.weight;
            total_Lift += wingData.lift;
            total_AirControl += wingData.airControl;
            total_AirRotationalControl += wingData.airRotationalControl;
        }
        if (tireData != null)
        {
            total_Weight += tireData.weight;
            total_Torque += tireData.torque;
        }
        if (rocketData != null)
        {
            total_Weight += rocketData.weight;
            total_JetThrust += rocketData.jetThrust;
            total_RocketTime += rocketData.time; // ロケット噴射時間を追加
        }

        Debug.Log($"総重量: {total_Weight}, 浮力: {total_Lift}, コントロール: {total_AirControl}, 回転制御: {total_AirRotationalControl}, 地上加速: {total_Torque}, 空中加速: {total_JetThrust}, ロケット噴射時間: {total_RocketTime}, 空気抵抗: {total_AirResistance}");

        rb.linearDamping = total_AirResistance; //空気抵抗なんだけど、まだどうするか迷ってる。浮力と空気抵抗の区別ができん。。。
        rb.mass = total_Weight;
        Debug.Log(rb.mass);
    }

    // ステータスの値を取得する関数ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    //全体の重さ
    public float GetWeight() => total_Weight;

    //Wingのデータ
    public float GetLift() => total_Lift;
    public float GetAirControl() => total_AirControl;
    public float GetAirControlRotational() => total_AirRotationalControl;

    //Tireのデータ
    public float GetGroundAcceleration() => total_Torque;

    //Rocketのデータ
    public float GetJetThrust() => total_JetThrust;
    public float GetRocketTime() => total_RocketTime;

    //Bodyのデータ
    public float GetAirResistance() => total_AirResistance;

    //プレイヤーの方向を変えるメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void PlayerAngle()
    {
        // 入力取得（Wキー：1, Sキー：-1, それ以外：0）
        float input = Input.GetAxis("Vertical");

        // 現在の角速度を取得
        float currentAngularVelocity = rb.angularVelocity;

        if (input != 0)
        {
            // 回転速度が上限以下の場合のみトルクを適用
            if ((input > 0 && currentAngularVelocity < total_AirRotationalControl) ||
                (input < 0 && currentAngularVelocity > -total_AirRotationalControl))
            {
                float torque = input * total_AirControl;
                rb.AddTorque(torque);
            }
        }
        else
        {
            // キー入力がない場合、角速度を徐々に減衰させる
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, Time.deltaTime * 5f);

            // デバッグログ（オプション）
            // Debug.Log($"Angular velocity reducing to: {rb.angularVelocity}");
        }
    }


    //ロケットメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void OnLeftClick()
    {
        if (!finishRocketTime) return;
        // 左Shiftキーを押している間はisLeftClickをtrueに設定
        if (Input.GetKey(KeyCode.LeftShift))
        {
            crrentRocketTime += Time.deltaTime;
            if (total_RocketTime > crrentRocketTime)
            {
                AddForce(Vector2.right);
                // Debug.Log($"{GetRocketTime()} + {crrentRocketTime}");
            }
            else{finishRocketTime = false;}
        }
    }
    
    //ロケットメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private void AddForce(Vector2 direction)
    {
        // 回転に基づいて力の方向を計算
        Vector2 forceDirection = new Vector2(Mathf.Cos(playerAngle * Mathf.Deg2Rad), Mathf.Sin(playerAngle * Mathf.Deg2Rad));

        // 力を加える（推進力を掛けて）
        rb.AddForce(forceDirection * total_RocketTime, ForceMode2D.Force);
    }

    //車輪メソッド
    private void GroundAddForce()
    {
        if (!groundCheck) return;
        // 水平方向の入力取得（Aキー: -1, Dキー: 1, それ以外: 0）
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            // プレイヤーの回転に基づいた右方向を計算
            Vector2 rightDirection = new Vector2(Mathf.Cos(playerAngle * Mathf.Deg2Rad), Mathf.Sin(playerAngle * Mathf.Deg2Rad));

            // 入力に基づいて力を計算
            Vector2 force = rightDirection * input * total_Torque;

            // 力を加える
            rb.AddForce(force, ForceMode2D.Force);
            // デバッグログで力の情報を出力
            Debug.Log($"Applied Ground Force: {force}, Input: {input}");
        }
    }

}
