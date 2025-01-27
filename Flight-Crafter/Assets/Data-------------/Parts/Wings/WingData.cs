using UnityEngine;

[CreateAssetMenu(fileName = "New Wing", menuName = "Parts/Wing")]
public class WingData : PartData
{
    [Header("揚力")]
    [Tooltip("値が高いほど水平方向に向いているときの、垂直方向の浮力が大きくなります。")]
    public float lift;       // 揚力
    
    [Header("空中制御")]
    [Tooltip("値が高いほど、空中での操作性が良くなります。")]
    public float airControl; //空中制御

    [Header("回転制御")]
    [Tooltip("値が高いほど空中での回転の上限が高くなります。")]
    public float airRotationalControl;

    [Header("推進力%")]
    [Tooltip("値が高いほど、ベクトル推進力が強くなります。")]
    public float propulsionPower; // 推進力の大きさ
}
