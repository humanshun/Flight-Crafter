using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusNameText; // ステータス名のテキスト
    [SerializeField] private TextMeshProUGUI statusValueText; // ステータス値のテキスト
    public void Setup(string displayName, float value)
    {
        statusNameText.text = displayName; // ステータス名を設定
        statusValueText.text = value.ToString(); // ステータス値を設定
    }
}
