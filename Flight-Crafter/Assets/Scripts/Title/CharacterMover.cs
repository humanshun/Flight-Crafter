using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

public class CharacterMover : MonoBehaviour
{
    [System.Serializable]
    public class MoveStep
    {
        public string name;                      // ステップ名（任意）
        public AnimationClip clip;               // 再生するアニメーションクリップ
        public Vector3 startPosition;            // 開始位置
        public Vector3 endPosition;              // 終了位置
        public Vector3 scale = new Vector3(1, 1, 1);
        public float moveDuration = 1.2f;        // 移動時間
        public float waitBeforeMove = 0.3f;      // アニメーション前の待機
        public float waitAfterMove = 0.5f;       // 移動後の待機
        public UnityEvent onStepAction;          // ★ステップ実行時に呼び出されるイベント
    }

    public Animator animator;
    public List<MoveStep> steps = new();

    void Start()
    {
        StartCoroutine(PlaySequence(transform));
    }

    public IEnumerator PlaySequence(Transform characterTransform)
    {
        foreach (var step in steps)
        {
            characterTransform.position = step.startPosition;

            yield return new WaitForSeconds(step.waitBeforeMove);

            if (animator != null && step.clip != null)
            {
                animator.Play(step.clip.name);
            }

            // ★任意のイベント呼び出し（インスペクターで設定可能）
            step.onStepAction?.Invoke();

            characterTransform.localScale = step.scale;

            yield return characterTransform.DOMove(step.endPosition, step.moveDuration)
                .SetEase(Ease.Linear)
                .WaitForCompletion();

            yield return new WaitForSeconds(step.waitAfterMove);
        }
    }

    public void AnimStop()
    {
        animator.speed = 0;
    }
}
