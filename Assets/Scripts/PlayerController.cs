using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardMovementSpeed;
    
    void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * forwardMovementSpeed));
    }
}
