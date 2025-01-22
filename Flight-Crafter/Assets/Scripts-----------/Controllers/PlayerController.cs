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

    public GameObject rocketThrust;

    // 各パーツデータを保持するクラスへの参照ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private BodyData bodyData; // 本体のデータ
    private RocketData rocketData; // ロケットのデータ
    private TireData tireData; // タイヤのデータ
    private WingData wingData; // 翼のデータ

    // パーツごとの合計値を計算・保持するための変数ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private float total_Weight; // 総重量
    private float total_AirResistance; //総空気抵抗
    private float total_JetThrust; // 総空中加速度
    private float total_Torque; // 総地上加速度
    private float total_Lift; // 総浮力
    private float total_AirControl; //空中コントロール
    private float total_AirRotationalControl; //回転制御
    private float total_PropulsionPower;
    private float total_RocketTime; // ロケットの合計噴射時間

    // プレイヤーの制御に必要なパラメータ
    private Rigidbody2D rb; // プレイヤーのRigidbody2D
    [SerializeField] private float playerAngle;
    private float crrentRocketTime; //現在のロケット噴射時間
    private bool finishRocketTime = true; //ロケットが使えるかどうか
    public bool groundCheck; //Groundについているかどうか
    private Vector2 rightDirection;// プレイヤーの回転に基づいた右方向


    float MovY,MovX = 1;

    void Start()//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    {
        rocketThrust.SetActive(false);
        // Rigidbody2D を取得
        rb = GetComponent<Rigidbody2D>();

        // パーツデータを初期化
        InitializeParts();

        // 初期ステータスを計算
        UpdateStats();
    }
    void Update() //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    {
        MovY = Input.GetAxis("Vertical");

        // 左クリックの状態をチェック
        if (rocketPrefab != null) {OnLeftClick();}

        // プレイヤーのローカルZ回転を取得
        playerAngle = transform.localRotation.eulerAngles.z;

        GroundAddForce();

        // プレイヤーの回転に基づいた右方向を計算
        rightDirection = new Vector2(Mathf.Cos(playerAngle * Mathf.Deg2Rad), Mathf.Sin(playerAngle * Mathf.Deg2Rad));
    }

    void FixedUpdate()
    {
        // プレイヤーの向きを制御する
        if (wingPrefab != null) {PlayerAngle();}
    }

    // パーツデータを初期化
    void InitializeParts()//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
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

    // プレイヤーのパーツ性能を集計//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void UpdateStats()
    {
        total_Weight = 0f;
        total_Lift = 0f;
        total_Torque = 0f;
        total_JetThrust = 0f;
        total_RocketTime = 0f;

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
            total_PropulsionPower += wingData.propulsionPower;
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

        Debug.Log($"総重量: {total_Weight},総空気抵抗{total_AirResistance}, 揚力: {total_Lift}, コントロール: {total_AirControl}, 回転制御: {total_AirRotationalControl}, 推進力: {total_PropulsionPower}, 地上加速: {total_Torque}, 空中加速: {total_JetThrust}, ロケット噴射時間: {total_RocketTime}");

        rb.linearDamping = total_AirResistance; //空気抵抗。下を向くほどその方向に加速するように。
        rb.mass = total_Weight;
        Debug.Log(rb.mass);
    }

    // ステータスの値を取得する関数ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    //全体の重さ
    public float GetWeight() => total_Weight;

    //Bodyのデータ
    public float GetAirResistance() => total_AirResistance;

    //Wingのデータ
    public float GetLift() => total_Lift;
    public float GetAirControl() => total_AirControl;
    public float GetAirControlRotational() => total_AirRotationalControl;
    public float GetPropulsionPower() => total_PropulsionPower;

    //Tireのデータ
    public float GetGroundAcceleration() => total_Torque;

    //Rocketのデータ
    public float GetJetThrust() => total_JetThrust;
    public float GetRocketTime() => total_RocketTime;

    //プレイヤーの方向を変えるメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void PlayerAngle()
    {
        if (groundCheck != false) return;

        // 水平方向の加速を計算（プレイヤーのローカル座標の右方向に加速）
        Vector2 Vel = transform.right * (MovX * total_JetThrust);
        rb.AddForce(Vel);

        // 回転に対するトルクを加える（AddTorqueを使用）
        float torque = MovY * total_AirControl;  // MovYとtotal_AirControlで回転力を計算
        rb.AddTorque(torque);

        // 垂直方向の力を計算（進行方向が下向きのときの揚力）
        float thrustForce = Vector2.Dot(rb.linearVelocity, rb.GetRelativeVector(Vector2.down)) * 2.0f;

        // 上方向に力を加えるためのベクトルを計算
        Vector2 relForce = Vector2.up * thrustForce;

        // Rigidbody2Dに揚力を加える
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
                rocketThrust.SetActive(true);
                // Debug.Log($"{GetRocketTime()} + {crrentRocketTime}");
            }
            else
            {
                rocketThrust.SetActive(false);
                finishRocketTime = false;
            }
        }
        else
        {
            rocketThrust.SetActive(false);
        }
    }
    
    //ロケットメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    private void AddForce(Vector2 direction)
    {
        // 回転に基づいて力の方向を計算
        Vector2 forceDirection = new Vector2(Mathf.Cos(playerAngle * Mathf.Deg2Rad), Mathf.Sin(playerAngle * Mathf.Deg2Rad));

        // 力を加える（推進力を掛けて）
        rb.AddForce(forceDirection * total_JetThrust, ForceMode2D.Force);
    }

    //車輪メソッド
    private void GroundAddForce()//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    {
        if (!groundCheck) return;
        // 水平方向の入力取得（Aキー: -1, Dキー: 1, それ以外: 0）
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            // 入力に基づいて力を計算
            Vector2 force = rightDirection * input * total_Torque;

            // 力を加える
            rb.AddForce(force, ForceMode2D.Force);
        }
    }
}
