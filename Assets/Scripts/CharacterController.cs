using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float sidewaysMovementSpeed;

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
                    new Vector3(touch.deltaPosition.normalized.x * sidewaysMovementSpeed, 0f, 0f);
                float smoothTime = 0.15f;

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

        CollectedCollectiblesManager.Instance.SwerveFollow();
    }

    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(_rb.position.x, minX, maxX), transform.position.y,
            transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money") || other.CompareTag("Gold") || other.CompareTag("Diamond"))
        {
            CollectedCollectiblesManager.Instance.AddCollectible(other.gameObject);
        }
    }
}