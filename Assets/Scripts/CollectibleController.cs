using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectibleController : MonoBehaviour
{
    public enum CollectibleType
    {
        Money,
        Gold,
        Diamond
    }

    [SerializeField] private GameObject money;
    [SerializeField] private GameObject gold;
    [SerializeField] private GameObject diamond;

    private MeshRenderer _moneyMeshRenderer;
    private MeshRenderer _goldMeshRenderer;
    private MeshRenderer _diamondMeshRenderer;
    private MeshRenderer _currentMeshRenderer;

    private ParticleSystem _moneyParticleSystem;
    private ParticleSystem _goldParticleSystem;
    private ParticleSystem _diamondParticleSystem;
    private ParticleSystem _currentParticleSystem;

    private BoxCollider _boxCollider;


    public bool isCollected = false;
    public CollectibleType currentType = CollectibleType.Money;

    private void Start()
    {
        _moneyMeshRenderer = money.GetComponent<MeshRenderer>();
        _goldMeshRenderer = gold.GetComponent<MeshRenderer>();
        _diamondMeshRenderer = diamond.GetComponent<MeshRenderer>();
        _currentMeshRenderer = _moneyMeshRenderer;

        _moneyParticleSystem = money.GetComponent<ParticleSystem>();
        _goldParticleSystem = gold.GetComponent<ParticleSystem>();
        _diamondParticleSystem = diamond.GetComponent<ParticleSystem>();
        _currentParticleSystem = _moneyParticleSystem;

        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnObstacleTriggered()
    {
        _currentMeshRenderer.enabled = false;
        _boxCollider.enabled = false;
        _currentParticleSystem.Play();
    }

    private void SwapCollectible()
    {
        if (currentType == CollectibleType.Money)
        {
            money.SetActive(false);
            gold.SetActive(true);
            _currentMeshRenderer = _goldMeshRenderer;
            _currentParticleSystem = _goldParticleSystem;
            currentType = CollectibleType.Gold;
        }
        else if (currentType == CollectibleType.Gold)
        {
            gold.SetActive(false);
            diamond.SetActive(true);
            _currentMeshRenderer = _diamondMeshRenderer;
            _currentParticleSystem = _diamondParticleSystem;
            currentType = CollectibleType.Diamond;
        }
    }

    public void OnATMTriggered()
    {
        _currentMeshRenderer.enabled = false;
        _boxCollider.enabled = false;
        _currentParticleSystem.Play();
        
        PlayerManager.Instance.IncreaseCollectedCollectiblesCount(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected) return;

        if (other.CompareTag("Fixed Obstacle"))
        {
            CollectedCollectiblesManager.Instance.OnFixedObstacleTriggerred(this);
            OnObstacleTriggered();
        }
        else if (other.CompareTag("Spinning Obstacle"))
        {
            CollectedCollectiblesManager.Instance.OnSpinningObstacleTriggered(this);
            OnObstacleTriggered();
        }
        else if (other.CompareTag("Card Obstacle"))
        {
            CollectedCollectiblesManager.Instance.OnCardObstacleTriggered(this);
            OnObstacleTriggered();
        }
        else if (other.CompareTag("Swinging Obstacle"))
        {
            CollectedCollectiblesManager.Instance.OnSwingingObstacleTriggered(this);
            OnObstacleTriggered();
        }
        else if (other.CompareTag("Barbed Obstacle"))
        {
            CollectedCollectiblesManager.Instance.OnBarbedObstacleTriggered(this);
            OnObstacleTriggered();
        }
        else if (other.CompareTag("Upgrade Gate"))
        {
            CollectedCollectiblesManager.Instance.OnUpgradeGateTriggered(this);
            SwapCollectible();
        }else if (other.CompareTag("ATM"))
        {
            CollectedCollectiblesManager.Instance.OnATMTriggered(this, other.gameObject);
        }else if (other.CompareTag("Conveyor Belt"))
        {
            _boxCollider.enabled = false;
            CollectedCollectiblesManager.Instance.OnConveyorBeltTriggered(this);
            PlayerManager.Instance.OnConveyorBeltTriggered(this);
        }
    }
}