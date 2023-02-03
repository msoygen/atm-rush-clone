using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private Vector3 _forward;
    private Vector3 _relativePosition;

    private ParticleSystem _particleSystem;
    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;

    private void Start()
    {
        _forward = transform.forward;
        _particleSystem = GetComponent<ParticleSystem>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fixed Obstacle"))
        {
            CollectedCollectiblesManager.Instance.OnFixedObstacleTriggerred(gameObject);
            _meshRenderer.enabled = false;
            _boxCollider.enabled = false;
            
            _particleSystem.Play();
        }else if (other.CompareTag("Spinning Obstacle"))
        {
            CollectedCollectiblesManager.Instance.OnSpinningObstacleTriggered(gameObject);
            _meshRenderer.enabled = false;
            _boxCollider.enabled = false;
            
            _particleSystem.Play();
        }
    }
}
