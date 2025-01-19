using UnityEngine;

public class GravityChange : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Playerタグの確認
        if (other.gameObject.CompareTag("Player"))
        {
            // Rigidbody2Dを取得
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.gravityScale = 1;
            }
        }
    }
}
