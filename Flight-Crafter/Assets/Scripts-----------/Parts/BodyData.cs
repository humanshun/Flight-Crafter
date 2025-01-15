using UnityEngine;

// ScriptableObjectとして作成可能にする
[CreateAssetMenu(fileName = "New Body", menuName = "Parts/Body")]
public class BodyData : PartData
{
    public float airResistance;  // 空気抵抗のプロパティ
}
