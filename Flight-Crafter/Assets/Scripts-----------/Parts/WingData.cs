using UnityEngine;

[CreateAssetMenu(fileName = "New Wing", menuName = "Parts/Wing")]
public class WingData : PartData
{
    [Header("浮力")]
    [Tooltip("値が高いほど水平方向に向いているときの、垂直方向の浮力が大きくなります。")]
    public float lift;       // 浮力
    
    [Header("空中制御")]
    [Tooltip("値が高いほど、空中での操作性が良くなります。")]
    public float airControl; //空中制御

    [Header("回転制御")]
    [Tooltip("値が高いほど空中での回転力が高くなります。")]
    public float airRotationalControl;
}
