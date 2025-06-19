using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A=-1, D=1
        Vector2 currentPos = rb.position;

        // 水平方向の移動（A/Dキー）
        if (h != 0)
        {
            currentPos.x += h * moveSpeed * Time.fixedDeltaTime;
            spriteRenderer.flipX = h > 0;
        }

        // Yが4未満なら、Y=4まで垂直に移動
        if (currentPos.y < 4f)
        {
            currentPos.y = Mathf.MoveTowards(currentPos.y, 4f, moveSpeed * Time.fixedDeltaTime);
        }

        // 実際に移動
        rb.MovePosition(currentPos);
    }
}
