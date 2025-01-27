using UnityEngine;
public class PartData : ScriptableObject
{
    [Header("パーツ名")]
    public string partName; // パーツ名
    [Header("重量")]
    [Tooltip("値が高いほど重力が大きくなります。")]
    public float weight;    // 重量（すべてのパーツに共通）
}
