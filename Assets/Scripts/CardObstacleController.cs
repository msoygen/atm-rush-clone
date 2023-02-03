using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardObstacleController : MonoBehaviour
{
    [SerializeField] private Transform cardTransform;

    private void Start()
    {
        Sequence loop = DOTween.Sequence();

        Vector3 startPos = cardTransform.localPosition + Vector3.right * 9;
        Vector3 endPos = cardTransform.localPosition;

        loop.Append(cardTransform.DOLocalMove(startPos, 2f).SetDelay(2f))
            .Append(cardTransform.DOLocalMove(endPos, 2f).SetDelay(2f));

        loop.SetLoops(-1, LoopType.Restart);
    }
}