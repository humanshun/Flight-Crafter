using System;
using Ricimi;
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
                Transition.LoadLevel("Custom", 2.0f, Color.black);
                break;
            case ButtonType.Continue:
                Transition.LoadLevel("Custom", 2.0f, Color.black);
                break;
        }
    }
}
