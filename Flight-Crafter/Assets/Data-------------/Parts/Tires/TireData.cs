using UnityEngine;

[CreateAssetMenu(fileName = "New Tire", menuName = "Parts/Tire")]
public class TireData : PartData
{
    [Header("地上加速度")]
    [Tooltip("値が高いほど地上での加速が大きくなります。")]
    public Torque torque;  // トルク

    public class Torque
    {
        [HideInInspector] public string displayName = "地上加速度";
        public float value;
    }
}
