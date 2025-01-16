using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // 各パーツデータを保持するクラスへの参照
    public BodyData body; // 本体のデータ
    public WingData wing; // 翼のデータ
    public TireData tire; // タイヤのデータ
    public RocketData rocket; // ロケットのデータ


    // パーツごとの合計値を計算・保持するための変数
    private float totalWeight; // 総重量
    private float totalLift; // 総浮力
    private float totalGroundAcceleration; // 総地上加速度
    private float totalAirAcceleration; // 総空中加速度


    // private UnityEngine.Camera mainCamera; // カメラを手動で指定

    // プレイヤーの制御に必要なパラメータ
    [SerializeField] private float rocketPower = 10f; // ロケットの推進力
    [SerializeField] private float wheelRotationAngle = 0.2f; // プレイヤーの回転角度
    private Rigidbody2D rb; // プレイヤーのRigidbody2D
    private bool isLeftClick = false; // 左クリック状態を保持するフラグ
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
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D がアタッチされていません。このスクリプトは Rigidbody2D を必要とします。");
        }
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
        PlayerAngle();

        // 左クリックの状態をチェック
        OnLeftClick();

        // 左クリック状態がtrueの場合、右方向に力を加える
        if (isLeftClick)
        {
            AddForce(Vector2.right);
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
    public void UpdateStats() // 各パーツのステータスを集計する
    {
        // 重量と空気抵抗を取得
        totalWeight = body.weight;
        float totalAirResistance = body.airResistance;

        // 翼の浮力と重量を加算
        totalLift = 0f;
        totalLift += wing.lift;
        totalWeight += wing.weight;

        // タイヤの地上加速度と重量を加算
        totalGroundAcceleration = 0f;
        totalGroundAcceleration += tire.groundAcceleration;
        totalWeight += tire.weight;

        // ロケットの空中加速度と重量を加算
        totalAirAcceleration = 0f;
        totalAirAcceleration += rocket.airAcceleration;
        totalWeight += rocket.weight;

        // デバッグ用に各ステータスをログ出力
        Debug.Log($"Weight: {totalWeight}, Lift: {totalLift}, Ground Acceleration: {totalGroundAcceleration}, Air Acceleration: {totalAirAcceleration}, Air Resistance: {totalAirResistance}");
    }

    // ステータスの値を取得する関数
    public float GetWeight() => totalWeight;
    public float GetLift() => totalLift;
    public float GetGroundAcceleration() => totalGroundAcceleration;
    public float GetAirAcceleration() => totalAirAcceleration;

    public void PlayerAngle()
    {
        // Wキーを押したら時計回りに回転
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation *= Quaternion.Euler(0, 0, wheelRotationAngle);
        }
        // Sキーを押したら反時計回りに回転
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation *= Quaternion.Euler(0, 0, -wheelRotationAngle);
        }
    }

    public void OnLeftClick()
    {
        // 左Shiftキーを押している間はisLeftClickをtrueに設定
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isLeftClick = true;
        }
        else
        {
            isLeftClick = false;
        }
    }
    
    private void AddForce(Vector2 direction)
    {
        // プレイヤーのローカルZ回転を取得
        float angleInRadians = transform.localRotation.eulerAngles.z * Mathf.Deg2Rad;

        // 回転に基づいて力の方向を計算
        Vector2 forceDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));

        // 力を加える（推進力を掛けて）
        rb.AddForce(forceDirection * rocketPower, ForceMode2D.Force);
    }
}
