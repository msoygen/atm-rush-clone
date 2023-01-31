using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [FormerlySerializedAs("collectibleManager")] [SerializeField] private CollectedCollectiblesManager collectedCollectiblesManager;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float sidewaysMovementSpeed;
    [SerializeField] private float forwardMovementSpeed;

    [SerializeField] private Rigidbody characterRb;

    [SerializeField] private Transform characterTransform;

    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;
    private float _swipeSpeed;

    private void Start()
    {
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

                characterRb.velocity = Vector3.SmoothDamp(characterRb.velocity, targetVelocity, ref currentVelocity, smoothTime);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Began)
            {
                characterRb.velocity = Vector3.zero;
            }
        }
        else
        {
            characterRb.velocity = Vector3.zero;
        }
        collectedCollectiblesManager.SwerveFollow();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * forwardMovementSpeed));

        characterTransform.position = new Vector3(Mathf.Clamp(characterRb.position.x, minX, maxX), transform.position.y,
            characterTransform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money") || other.CompareTag("Gold") || other.CompareTag("Diamond"))
        {
            collectedCollectiblesManager.AddCollectible(other.gameObject);
        }
    }
}