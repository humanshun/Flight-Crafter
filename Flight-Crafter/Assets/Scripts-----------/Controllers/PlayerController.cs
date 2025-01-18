using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System;

public class PlayerController : MonoBehaviour
{
    // パーツのプレハブ
    public GameObject bodyPrefab;
    public GameObject rocketPrefab;
    public GameObject tirePrefab;
    public GameObject wingPrefab;

    // 各パーツデータを保持するクラスへの参照
    private BodyData bodyData; // 本体のデータ
    private RocketData rocketData; // ロケットのデータ
    private TireData tireData; // タイヤのデータ
    private WingData wingData; // 翼のデータ
    


    // パーツごとの合計値を計算・保持するための変数
    private float total_Weight; // 総重量
    private float total_JetThrust; // 総空中加速度
    private float total_Torque; // 総地上加速度
    private float total_Lift; // 総浮力
    private float total_RocketTime; // ロケットの合計噴射時間
    private float total_AirResistance; //総空気抵抗
    private float time;
    private bool finishRocketTime = true;
    


    // private UnityEngine.Camera mainCamera; // カメラを手動で指定

    // プレイヤーの制御に必要なパラメータ
    [SerializeField] private float playerAngle;
    private Rigidbody2D rb; // プレイヤーのRigidbody2D





    public float rotationSpeed = 100f; // 回転速度
    public float maxRotationAngle = 90f; // 最大回転角度（±）

    private float currentRotationAngle = 0f; // 現在の回転角度






    void Start()
    {
        // //
        // // mainCameraが未設定の場合、自動で取得
        // if (mainCamera == null)
        // {
        //     mainCamera = FindFirstObjectByType<UnityEngine.Camera>();
        // }

        // if (mainCamera == null)
        // {
        //     Debug.LogError("カメラが見つかりません。カメラをインスペクターで指定してください。");
        // }

        // Rigidbody2D を取得
        rb = GetComponent<Rigidbody2D>();

        // パーツデータを初期化
        InitializeParts();

        // 初期ステータスを計算
        UpdateStats();
    }
    void Update()
    {
    //    //インプットシステム
    //    //マウスのワールド座標を取得
    //     Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

    //     //プレイヤーとマウスの位置を2D平面で比較
    //     Vector2 direction = (mousePosition - transform.position).normalized;

    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     transform.rotation = Quaternion.Euler(0, 0, angle);

        // プレイヤーの向きを制御する
        if (wingPrefab != null) {PlayerAngle();}

        // 左クリックの状態をチェック
        if (rocketPrefab != null) {OnLeftClick();}

        // プレイヤーのローカルZ回転を取得
        playerAngle = transform.localRotation.eulerAngles.z;
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

    // //インプットシステム
    // public void OnLeftClick(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         Debug.Log("aa");
    //         isLeftClick = true;
    //     }
    //     else if (context.canceled)
    //     {
    //         isLeftClick = false;
    //     }
    // }

    // //インプットシステム
    // public void OnWheel(InputValue inputValue)
    // {
    //     float scrollValue = inputValue.Get<float>();

    //     if (scrollValue > 0) // スクロールアップ
    //     {
    //         transform.rotation *= Quaternion.Euler(0, 0, wheelRotationAngle);
    //     }
    //     else if (scrollValue < 0) // スクロールダウン
    //     {
    //         transform.rotation *= Quaternion.Euler(0, 0, -wheelRotationAngle);
    //     }
    // }

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

        Debug.Log($"総重量: {total_Weight}, 浮力: {total_Lift}, 地上加速: {total_Torque}, 空中加速: {total_JetThrust}, ロケット噴射時間: {total_RocketTime}, 空気抵抗: {total_AirResistance}");

        rb.mass = total_Weight;
        Debug.Log(rb.mass);
    }

    // ステータスの値を取得する関数ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public float GetWeight() => total_Weight;
    public float GetLift() => total_Lift;
    public float GetGroundAcceleration() => total_Torque;
    public float GetJetThrust() => total_JetThrust;
    public float GetRocketTime() => total_RocketTime;
    public float GetAirResistance() => total_AirResistance;

    //プレイヤーの方向を変えるメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void PlayerAngle()
    {
        // 入力取得（Wキー：1, Sキー：-1, それ以外：0）
        float input = Input.GetAxis("Vertical");

        // 回転角度を計算
        float rotationChange = input * rotationSpeed * Time.deltaTime;

        // 回転制限を考慮
        float newRotationAngle = Mathf.Clamp(currentRotationAngle + rotationChange, -maxRotationAngle, maxRotationAngle);

        // 実際に回転を適用
        float appliedRotation = newRotationAngle - currentRotationAngle; // 実際の回転分
        transform.Rotate(Vector3.forward, appliedRotation);

        // 現在の回転角度を更新
        currentRotationAngle = newRotationAngle;
    }

    //ロケットメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void OnLeftClick()
    {
        if (!finishRocketTime) return;
        // 左Shiftキーを押している間はisLeftClickをtrueに設定
        if (Input.GetKey(KeyCode.LeftShift))
        {
            time += Time.deltaTime;
            if (total_RocketTime > time)
            {
                AddForce(Vector2.right);
                Debug.Log($"{GetRocketTime()} + {time}");
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

}
