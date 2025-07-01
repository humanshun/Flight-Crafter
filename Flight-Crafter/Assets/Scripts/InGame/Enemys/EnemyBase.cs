using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("基本ステータス")]
    [SerializeField] protected float maxHP = 100f; // 最大HP
    protected float currentHP; // 現在のHP

    [Header("移動速度")]
    [SerializeField] protected float moveSpeed = 10f;

    [Header("攻撃力")]
    [SerializeField] protected float attackPower = 5f;

    [Header("衝突時のダメージ")]
    [SerializeField] protected float selfDamageOnCollision = 5f;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Attack() { }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController2 player = collision.GetComponentInParent<PlayerController2>();
            if (player != null)
            {
                player.TakeDamage(selfDamageOnCollision);

                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
                }
            }
        }

        TakeDamage(selfDamageOnCollision);
    }
}
