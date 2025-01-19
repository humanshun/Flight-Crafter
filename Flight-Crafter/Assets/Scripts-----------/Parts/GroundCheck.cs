using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        // 親オブジェクトから PlayerController を取得
        playerController = GetComponentInParent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController が見つかりません。GroundCheck を適切に設定してください。");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") && playerController != null)
        {
            playerController.groundCheck = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") && playerController != null)
        {
            playerController.groundCheck = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground") && playerController != null)
        {
            playerController.groundCheck = false;
        }
    }
}
