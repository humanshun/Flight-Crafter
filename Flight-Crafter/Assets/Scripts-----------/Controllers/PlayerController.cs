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
    private float totalWeight; // 総重量
    private float totalAirAcceleration; // 総空中加速度
    private float totalGroundAcceleration; // 総地上加速度
    private float totalLift; // 総浮力
    private float totalRocketTime; // ロケットの合計噴射時間
    private float totalAirResistance;
    private float time;
    private bool finishRocketTime = true;
    


    // private UnityEngine.Camera mainCamera; // カメラを手動で指定

    // プレイヤーの制御に必要なパラメータ
    [SerializeField] private float rocketPower = 10f; // ロケットの推進力
    [SerializeField] private float wheelRotationAngle = 0.2f; // プレイヤーの回転角度
    [SerializeField] private float playerAngle;
    private Rigidbody2D rb; // プレイヤーのRigidbody2D
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
                Debug.Log($"Tireデータを取得: {tireData.weight}, {tireData.groundAcceleration}");
            }
        }

        if (rocketPrefab != null)
        {
            Part rocketPart = rocketPrefab.GetComponent<Part>();
            if (rocketPart != null && rocketPart.partData is RocketData rocket)
            {
                rocketData = rocket;
                Debug.Log($"Rocketデータを取得: {rocketData.weight}, {rocketData.airAcceleration}, {rocketData.time}");
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
        totalWeight = 0f;
        totalLift = 0f;
        totalGroundAcceleration = 0f;
        totalAirAcceleration = 0f;
        totalRocketTime = 0f;
        totalAirResistance = 0f;

        if (bodyData != null)
        {
            totalWeight += bodyData.weight;
            totalAirResistance += bodyData.airResistance;
        }
        if (wingData != null)
        {
            totalWeight += wingData.weight;
            totalLift += wingData.lift;
        }
        if (tireData != null)
        {
            totalWeight += tireData.weight;
            totalGroundAcceleration += tireData.groundAcceleration;
        }
        if (rocketData != null)
        {
            totalWeight += rocketData.weight;
            totalAirAcceleration += rocketData.airAcceleration;
            totalRocketTime += rocketData.time; // ロケット噴射時間を追加
        }

        Debug.Log($"総重量: {totalWeight}, 浮力: {totalLift}, 地上加速: {totalGroundAcceleration}, 空中加速: {totalAirAcceleration}, ロケット噴射時間: {totalRocketTime}, 空気抵抗: {totalAirResistance}");

        rb.mass = totalWeight;
        Debug.Log(rb.mass);
    }

    // ステータスの値を取得する関数ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public float GetWeight() => totalWeight;
    public float GetLift() => totalLift;
    public float GetGroundAcceleration() => totalGroundAcceleration;
    public float GetAirAcceleration() => totalAirAcceleration;
    public float GetRocketTime() => totalRocketTime;
    public float GetAirResistance() => totalAirResistance;

    //プレイヤーの方向を変えるメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void PlayerAngle()
    {
        //90以下の時にtrue
        //270以上の時にtrue
        if (playerAngle <= 90 || 270 <= playerAngle)
        {
            // Wキーを押したら時計回りに回転
            if (Input.GetKey(KeyCode.W))
            {
                transform.rotation *= Quaternion.Euler(0, 0, wheelRotationAngle);
            }
        }
        if (270 <= playerAngle || playerAngle <= 90)
        {
            // Sキーを押したら反時計回りに回転
            if (Input.GetKey(KeyCode.S))
            {
                transform.rotation *= Quaternion.Euler(0, 0, -wheelRotationAngle);
            }
        }
    }

    //ロケットメソッドーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void OnLeftClick()
    {
        if (!finishRocketTime) return;
        // 左Shiftキーを押している間はisLeftClickをtrueに設定
        if (Input.GetKey(KeyCode.LeftShift))
        {
            time += Time.deltaTime;
            if (totalRocketTime > time)
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
        Vector2 forceDirection = new Vector2(Mathf.Cos(playerAngle), Mathf.Sin(playerAngle));

        // 力を加える（推進力を掛けて）
        rb.AddForce(forceDirection * rocketPower, ForceMode2D.Force);

        // if (playerAngle)
    }
}
