using UnityEngine;
using DG.Tweening;
using System;

public class CoinMover : MonoBehaviour
{
    private Transform target;
    private float moveDuration;
    public void Init(Transform targetTransform, float duration)
    {
        target = targetTransform;
        moveDuration = duration;

        Sequence seq = DOTween.Sequence();

        // ① 最初にポップ演出（出現時拡大）
        transform.localScale = Vector3.zero;
        seq.Append(transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));

        // ② 出現後に少し静止する (ここが追加部分)
        seq.AppendInterval(0.2f);  // 0.1秒停止（ここは好みで調整）

        // ③ 逆方向に少し移動して弾ける感じにする
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 reverseOffset = transform.position - direction * 200.0f;

        seq.Append(transform.DOMove(reverseOffset, 0.3f).SetEase(Ease.OutQuad));

        // ④ 目的地に向かって移動開始
        seq.Append(transform.DOMove(target.position, moveDuration)
            .SetEase(Ease.InOutQuad));

        // ⑤ 目的地に着いたら消滅
        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
