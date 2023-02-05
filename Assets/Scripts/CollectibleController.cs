using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectibleController : MonoBehaviour
{
    enum CollectibleType
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
    private CollectibleType _currentType = CollectibleType.Money;

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
        if (_currentType == CollectibleType.Money)
        {
            money.SetActive(false);
            gold.SetActive(true);
            _currentMeshRenderer = _goldMeshRenderer;
            _currentParticleSystem = _goldParticleSystem;
            _currentType = CollectibleType.Gold;
        }
        else if (_currentType == CollectibleType.Gold)
        {
            gold.SetActive(false);
            diamond.SetActive(true);
            _currentMeshRenderer = _diamondMeshRenderer;
            _currentParticleSystem = _diamondParticleSystem;
            _currentType = CollectibleType.Diamond;
        }
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
            SwapCollectible();
        }
    }
}