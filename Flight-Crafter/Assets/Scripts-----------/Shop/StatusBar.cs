using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusNameText; // ステータス名のテキスト
    [SerializeField] private TextMeshProUGUI statusValueText; // ステータス値のテキスト
    [SerializeField] private RectTransform statusValue;
    public void Setup(string displayName, float value)
    {
        statusNameText.text = displayName; // ステータス名を設定
        statusValueText.text = value.ToString(); // ステータス値を設定

        statusValue.anchorMax = new Vector2(value / 100, statusValue.anchorMax.y); // ステータス値に応じてUIを調整
    }
}
