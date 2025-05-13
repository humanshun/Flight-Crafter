using System.Collections.Generic;
using UnityEngine;

// ScriptableObjectとして作成可能にする
[CreateAssetMenu(fileName = "New Body", menuName = "Parts/Body")]
public class BodyData : PartData
{
    [Header("空気抵抗")]
    [Tooltip("値が高いほど空気抵抗が大きくなります。")]
    public AirResistance airResistance;  // 抗力のプロパティ

    [Header("車輪の位置")]
    [Tooltip("車輪の位置を指定します。")]
    public Vector3 rightTirePosition;  // 車輪の位置
    public Vector3 leftTirePosition;   // 車輪の位置

    public class AirResistance
    {
        [HideInInspector] public string displayName = "空気抵抗";
        public float value;
    }
}
