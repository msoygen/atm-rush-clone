using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    [SerializeField] public float minX;
    [SerializeField] public float maxX;
    [SerializeField] public float sidewaysMovementSpeed;
    [SerializeField] public float forwardMovementSpeed;
    
    [SerializeField] public Transform characterTransform;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
