using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObstacleController : MonoBehaviour
{
    public float speed = 10.0f;

    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}