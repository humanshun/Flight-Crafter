using Cysharp.Threading.Tasks;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] string bord;
    [SerializeField] CharacterBird characterBird1;
    [SerializeField] CharacterBird characterBird2;

    private void Start()
    {
        characterBird1.stopPosition = -2;
        characterBird2.stopPosition = 2;
    }

    public async void End()
    {
        characterBird1.stopPosition = 14;
        characterBird2.stopPosition = 16;

        await UniTask.Delay(4000);

        if (this != null && gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
