using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] public float minX;
    [SerializeField] public float maxX;
    [SerializeField] public float sidewaysMovementSpeed;
    [SerializeField] public float forwardMovementSpeed;

    [SerializeField] public GameObject characterGameObject;
    [SerializeField] public GameObject collectedCollectiblesGameObject;
    
    [SerializeField] public Transform characterTransform;
    [SerializeField] public Transform collectedCollectiblesTransform;

    private float _sidewayMovementSpeedCopy;
    private float _forwardMovementSpeedCopy;

    private int _collectedMoney = 0;
    private int _collectedGold = 0;
    private int _collectedDiamond = 0;
    private int _totalCollectedCollectibles = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _sidewayMovementSpeedCopy = sidewaysMovementSpeed;
        _forwardMovementSpeedCopy = forwardMovementSpeed;
    }

    public void OnPlayerHitObstacle()
    {
        sidewaysMovementSpeed = 0f;
        forwardMovementSpeed = 0f;

        transform.DOMove(transform.position + Vector3.back * 4, 1f).OnComplete(() =>
        {
            sidewaysMovementSpeed = _sidewayMovementSpeedCopy;
            forwardMovementSpeed = _forwardMovementSpeedCopy;
        });

        CollectedCollectiblesManager.Instance.OnPlayerHitObstacle();
    }

    public void OnPlayerTriggeredConveyorBelt()
    {
        sidewaysMovementSpeed = 0f;
        forwardMovementSpeed = 0f;
    }

    public void OnConveyorBeltTriggered(CollectibleController collectible)
    {
        sidewaysMovementSpeed = 0f;
        IncreaseCollectedCollectiblesCount(collectible);
    }

    public void IncreaseCollectedCollectiblesCount(CollectibleController collectible)
    {
        if (collectible.currentType == CollectibleController.CollectibleType.Money)
        {
            _collectedMoney++;
        }else if (collectible.currentType == CollectibleController.CollectibleType.Gold)
        {
            _collectedGold++;
        }else if (collectible.currentType == CollectibleController.CollectibleType.Diamond)
        {
            _collectedDiamond++;
        }

        _totalCollectedCollectibles = _collectedMoney + _collectedGold + _collectedDiamond;
    }
    
}