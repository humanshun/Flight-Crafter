using UnityEngine;

public class EnemyCoinBird : EnemyBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController2 player = collision.GetComponentInParent<PlayerController2>();
            if (player != null)
            {
                // プレイヤーにコインを追加
                PlayerData.Instance.AddCoins(100);
                
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
                }
            }
        }
    }
}
