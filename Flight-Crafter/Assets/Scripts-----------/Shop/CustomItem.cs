using UnityEngine;
using UnityEngine.UI;

public class CustomItem : MonoBehaviour
{
    [SerializeField] private Image iconImage; // アイコン画像

    public void Setup(PartData part)
    {
        if (iconImage != null)
        {
            iconImage.sprite = part.partIconImage; // アイコン画像を設定
            iconImage.preserveAspect = true; // アスペクト比を保持する
        }
    }
}
