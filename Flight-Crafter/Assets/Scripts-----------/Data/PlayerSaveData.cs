using System.Collections.Generic;

[System.Serializable]

public class PlayerSaveData
{
    public int coins;
    public List<string> purchasedPartNames = new List<string>();
    public string currentPartName;
}