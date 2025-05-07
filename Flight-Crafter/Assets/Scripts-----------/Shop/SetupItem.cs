using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconImage;

    public void Setup(PartData part)
    {
        if (nameText != null) nameText.text = part.partName;
        if (costText != null) costText.text = $"¥{part.partCost}";
        if (descriptionText != null) descriptionText.text = part.partDescription;
        if (iconImage != null) iconImage.sprite = part.partIconImage;
        if (iconImage != null) iconImage.preserveAspect = true; // アスペクト比を保持する
    }
}
