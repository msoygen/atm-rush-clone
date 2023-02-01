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
            _relativePosition = transform.position - other.bounds.center;

            float dot = Vector3.Dot(_relativePosition, _forward) / _relativePosition.magnitude;

            if (dot > -0.1f && dot < 0.1f)
            {
                Debug.Log("Side collision");
            }
            else
            {
                Debug.Log("Front collision");
                
                CollectedCollectiblesManager.Instance.RemoveCollectible(gameObject);
                _meshRenderer.enabled = false;
                _boxCollider.enabled = false;

                _particleSystem.Play();
            }
        }
    }
}
