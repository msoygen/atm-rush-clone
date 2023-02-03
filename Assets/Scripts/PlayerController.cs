using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * (Time.fixedDeltaTime * PlayerManager.Instance.forwardMovementSpeed));
    }
}