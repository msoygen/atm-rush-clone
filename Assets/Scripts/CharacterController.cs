using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Rigidbody _rb;

    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;
    private float _swipeSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        animator.SetBool("run", true);
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 currentVelocity = Vector3.zero;
                Vector3 targetVelocity =
                    new Vector3(touch.deltaPosition.normalized.x * PlayerManager.Instance.sidewaysMovementSpeed, 0f,
                        0f);
                float smoothTime = 0.1f;

                _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Began)
            {
                _rb.velocity = Vector3.zero;
            }
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }

        transform.position = new Vector3(
            Mathf.Clamp(_rb.position.x, PlayerManager.Instance.minX, PlayerManager.Instance.maxX), transform.position.y,
            transform.position.z);
        
        CollectedCollectiblesManager.Instance.SwerveFollow();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            CollectedCollectiblesManager.Instance.AddCollectible(other.gameObject.GetComponent<CollectibleController>());
        }
        else if (other.CompareTag("Fixed Obstacle") || other.CompareTag("Spinning Obstacle") ||
                 other.CompareTag("Card Obstacle") || other.CompareTag("Swinging Obstacle") ||
                 other.CompareTag("Barbed Obstacle"))
        {
            PlayerManager.Instance.OnPlayerHitObstacle();
        }else if (other.CompareTag("ATM"))
        {
            other.transform.DOMoveY(-2.5f, 0.2f);
        }
    }
}