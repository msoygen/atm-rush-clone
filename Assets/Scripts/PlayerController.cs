using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float sidewaysMovementSpeed;
    [SerializeField] private float forwardMovementSpeed;

    private Rigidbody _rb;

    private Vector3 _startPos;
    private Vector3 _endPos;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * forwardMovementSpeed));
   /*     
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            _endPos = Input.mousePosition;

            Vector3 currentVelocity = Vector3.zero;
            Vector3 targetVelocity =
                new Vector3(Input.GetTouch(0).deltaPosition.normalized.x * sidewaysMovementSpeed, 0f, 0f);
            float smoothTime = 0.2f;

            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _startPos = Vector3.zero;
            _endPos = Vector3.zero;
            _rb.velocity = Vector3.zero;
        }
#endif
*/
        if (Input.touchCount > 0)
        {
            Vector3 currentVelocity = Vector3.zero;
            Vector3 targetVelocity =
                new Vector3(Input.GetTouch(0).deltaPosition.normalized.x * sidewaysMovementSpeed, 0f, 0f);
            float smoothTime = 0.2f;

            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }

    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(_rb.position.x, minX, maxX), transform.position.y, transform.position.z);
    }
}