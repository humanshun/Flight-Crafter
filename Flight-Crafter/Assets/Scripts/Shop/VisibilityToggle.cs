using UnityEngine;

public class VisibilityToggle : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    public void Toggle()
    {
        // targetObjectがnullでない場合、またはtargetObjectが非アクティブな場合
        if (targetObject != null && !targetObject.activeSelf)
        {
            // targetObjectをアクティブにする
            targetObject.SetActive(true);
        }
        else if (targetObject != null && targetObject.activeSelf)
        {
            // targetObjectを非アクティブにする
            targetObject.SetActive(false);
        }
    }
}
