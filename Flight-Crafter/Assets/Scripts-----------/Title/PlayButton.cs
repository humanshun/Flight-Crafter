using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public enum ButtonType
    {
        NewGame,
        Continue
    }
    [SerializeField] private ButtonType buttonType;

    void OnMouseDown()
    {
        switch (buttonType)
        {
            case ButtonType.NewGame:
                SceneManager.LoadScene("Custom");
                break;
            case ButtonType.Continue:
                SceneManager.LoadScene("Custom");
                break;
        }
    }
}
