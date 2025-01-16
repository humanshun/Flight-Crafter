using UnityEngine;

[CreateAssetMenu(fileName = "New Rocket", menuName = "Parts/Rocket")]
public class RocketData : PartData
{
    public float airAcceleration;  // 空中加速度
    public float time;
}
