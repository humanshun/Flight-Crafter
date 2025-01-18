using UnityEngine;

// ScriptableObjectとして作成可能にする
[CreateAssetMenu(fileName = "New Body", menuName = "Parts/Body")]
public class BodyData : PartData
{
    [Header("空気抵抗")]
    [Tooltip("値が高いほど空気抵抗が大きくなります。")]
    public float airResistance;  // 空気抵抗のプロパティ
}
