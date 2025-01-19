using UnityEngine;

// ScriptableObjectとして作成可能にする
[CreateAssetMenu(fileName = "New Body", menuName = "Parts/Body")]
public class BodyData : PartData
{
    [Header("抗力")]
    [Tooltip("値が高いほど空気抵抗が大きくなります。")]
    public float airResistance;  // 抗力のプロパティ
}
