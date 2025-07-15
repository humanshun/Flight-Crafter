using UnityEngine;

public class ItemHealth : ItemBase
{
    [SerializeField] private float healthAmount = 10f;
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerController2 player = collider.GetComponentInParent<PlayerController2>();
            if (player != null)
            {
                // 体力回復
                player.Heal(healthAmount);

                // TODO回復音
                // AudioManager.Instance.PlaySFX("SE_Coins");

                // TODOエフェクト
                gameObject.SetActive(false);
            }
        }
    }
}
