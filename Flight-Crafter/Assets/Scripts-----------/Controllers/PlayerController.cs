using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public BodyData body;
    public WingData wing;
    public TireData tire;
    public RocketData rocket;


    private float totalWeight;
    private float totalLift;
    private float totalGroundAcceleration;
    private float totalAirAcceleration;


    // private UnityEngine.Camera mainCamera; // カメラを手動で指定
    [SerializeField] private float rocketPower = 10f;
    [SerializeField] private float wheelRotationAngle = 0.2f;
    private Rigidbody2D rb;
    private bool isLeftClick = false;
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
        PlayerAngle();
        OnLeftClick();

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
    public void UpdateStats()
    {
        // 重量と空気抵抗
        totalWeight = body.weight;
        float totalAirResistance = body.airResistance;

        // 浮力
        totalLift = 0f;
        totalLift += wing.lift;
        totalWeight += wing.weight;

        // 地上加速度
        totalGroundAcceleration = 0f;
        totalGroundAcceleration += tire.groundAcceleration;
        totalWeight += tire.weight;

        // 空中加速度
        totalAirAcceleration = 0f;
        totalAirAcceleration += rocket.airAcceleration;
        totalWeight += rocket.weight;

        // デバッグ用ログ
        Debug.Log($"Weight: {totalWeight}, Lift: {totalLift}, Ground Acceleration: {totalGroundAcceleration}, Air Acceleration: {totalAirAcceleration}, Air Resistance: {totalAirResistance}");
    }

    public float GetWeight() => totalWeight;
    public float GetLift() => totalLift;
    public float GetGroundAcceleration() => totalGroundAcceleration;
    public float GetAirAcceleration() => totalAirAcceleration;

    public void PlayerAngle()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation *= Quaternion.Euler(0, 0, wheelRotationAngle);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation *= Quaternion.Euler(0, 0, -wheelRotationAngle);
        }
    }

    public void OnLeftClick()
    {
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

        // 力を加える
        rb.AddForce(forceDirection * rocketPower, ForceMode2D.Force);
    }
}
