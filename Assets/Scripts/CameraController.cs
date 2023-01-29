using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed;

    private void LateUpdate()
    {
        Vector3 targetPos = objectToFollow.position + 
                            objectToFollow.forward * offset.z + 
                            objectToFollow.right * offset.x + 
                            objectToFollow.up * offset.y;

        transform.position = targetPos;
    }
}
