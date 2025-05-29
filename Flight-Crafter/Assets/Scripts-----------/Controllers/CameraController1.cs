using UnityEngine;

public class CameraController1 : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float xOffset = 30f;
    [SerializeField] private float zOffset = 10f;

    private void OnEnable()
    {
        GameManager.OnInGamePlayerSpawned += OnPlayerSpawned;
    }
    private void OnDisable()
    {
        GameManager.OnInGamePlayerSpawned -= OnPlayerSpawned;
    }
    private void OnPlayerSpawned(CustomPlayer spawnedPlayer)
    {
        player = spawnedPlayer.gameObject;
    }
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x + xOffset, player.transform.position.y, zOffset);
        }
    }
}
