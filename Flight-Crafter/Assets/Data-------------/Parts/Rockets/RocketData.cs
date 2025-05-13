using UnityEngine;

[CreateAssetMenu(fileName = "New Rocket", menuName = "Parts/Rocket")]
public class RocketData : PartData
{
    [Header("噴射力")]
    [Tooltip("値が高いほどロケット噴射力が大きくなります。")]
    public JetThrust jetThrust;  // 噴射力

    [Header("噴射時間")]
    [Tooltip("値が高いほどロケット噴射時間が長くなります。")]
    public JetTime jetTime; //ロケット噴射時間

    public class JetThrust
    {
        [HideInInspector] public string displayName = "噴射力";
        public float value;
    }
    public class JetTime
    {
        [HideInInspector] public string displayName = "噴射時間";
        public float value;
    }
}
