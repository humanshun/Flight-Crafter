using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusNameText; // ステータス名のテキスト
    [SerializeField] private TextMeshProUGUI statusValueText; // ステータス値のテキスト
    [SerializeField] private RectTransform statusValue;
    public void Setup(PartType part, string displayName, float value, float maxValue)
    {
        statusNameText.text = displayName; // ステータス名を設定
        statusValueText.text = value.ToString("F1"); // ステータス値を設定
        float normalized = Mathf.Clamp01(value / maxValue);
        statusValue.anchorMax = new Vector2(normalized, statusValue.anchorMax.y); // ステータス値に応じてUIを調整
    }
}
