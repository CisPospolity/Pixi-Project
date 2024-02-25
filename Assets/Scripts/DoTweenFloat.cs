using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenFloat : MonoBehaviour
{
    [SerializeField]
    float floatDistance = 0.5f;
    [SerializeField]
    float duration = 1f;

    Sequence floatSequence;
    private void OnEnable()
    {
        DOTween.Init();

        Vector3 startPos = transform.position;

        floatSequence = DOTween.Sequence();

        floatSequence.Append(transform.DOMoveY(startPos.y + floatDistance, duration).SetEase(Ease.InOutSine));
        floatSequence.Append(transform.DOMoveY(startPos.y - floatDistance, duration).SetEase(Ease.InOutSine));

        floatSequence.SetLoops(-1, LoopType.Yoyo);

    }

    private void OnDestroy()
    {
        // Kill the sequence when the GameObject is destroyed
        if (floatSequence != null)
        {
            floatSequence.Kill();
        }
    }
}
