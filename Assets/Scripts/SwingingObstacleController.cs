using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SwingingObstacleController : MonoBehaviour
{
    [SerializeField] private Transform bodyTransform;

    private void Start()
    {
        Sequence loop = DOTween.Sequence();

        Vector3 startPos = new Vector3(0f, 0f, 180f);
        Vector3 endPos = new Vector3(0f, 0f, -180f);

        loop.Append(bodyTransform.DOLocalRotate(startPos, 2f, RotateMode.LocalAxisAdd).SetDelay(2f))
            .Append(bodyTransform.DOLocalRotate(endPos, 2f, RotateMode.LocalAxisAdd).SetDelay(2f));

        loop.SetLoops(-1, LoopType.Restart);
    }
}
