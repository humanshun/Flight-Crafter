using UnityEngine;

public class TitleButton : MonoBehaviour
{
    [SerializeField] private TitleManager titleManager;  // アタッチしておく
    [SerializeField] private TitleManager.TitleState targetState;  // 遷移先
    void Start()
    {
        titleManager = FindFirstObjectByType<TitleManager>();
    }

    void OnMouseDown()
    {
        // 左クリックされたときに呼ばれる
        if (titleManager != null)
        {
            titleManager.ChangeState(targetState);
        }
    }
}
