using UnityEngine;

[CreateAssetMenu(fileName = "New Rocket", menuName = "Parts/Rocket")]
public class RocketData : PartData
{
    [Header("噴射力")]
    [Tooltip("値が高いほどロケット噴射力が大きくなります。")]
    public float jetThrust;  // 噴射力

    [Header("噴射時間")]
    [Tooltip("値が高いほどロケット噴射時間が長くなります。")]
    public float time; //ロケット噴射時間
}
