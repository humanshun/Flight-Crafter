using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float xOffset = 30f;
    [SerializeField] private float zOffset = 10f;

    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x + xOffset, player.transform.position.y, zOffset);
        }
    }
}
