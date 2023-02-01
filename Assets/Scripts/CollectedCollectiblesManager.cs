using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectedCollectiblesManager : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;

    private List<GameObject> _collectedGameObjectsList;

    private void Start()
    {
        _collectedGameObjectsList = new List<GameObject>();
    }

    public void SwerveFollow()
    {
        Vector3 target = Vector3.zero;
        for (int i = 0; i < _collectedGameObjectsList.Count; i++)
        {
            if (i == 0)
            {
                target = _collectedGameObjectsList[i].transform.localPosition;
                target.x = characterTransform.localPosition.x;

                _collectedGameObjectsList[i].transform.localPosition = target;
            }
            else
            {
                target = _collectedGameObjectsList[i].transform.localPosition;
                target.x = _collectedGameObjectsList[i - 1].transform.localPosition.x;

                _collectedGameObjectsList[i].transform.DOLocalMove(target, 0.3f);
            }
        }
    }

    public void AddCollectible(GameObject collectible)
    {
        collectible.transform.SetParent(transform);
        if (_collectedGameObjectsList.Count == 0)
        {
            collectible.transform.position = new Vector3(characterTransform.position.x,
                collectible.transform.position.y,
                characterTransform.position.z + 5f);
        }
        else
        {
            collectible.transform.localPosition =
                new Vector3(_collectedGameObjectsList[^1].transform.localPosition.x,
                    collectible.transform.localPosition.y,
                    _collectedGameObjectsList[^1].transform.localPosition.z + 2f);
        }

        _collectedGameObjectsList.Add(collectible);

        collectible.AddComponent<CollectibleController>();

        ShakeCollectedGameObjects();
    }

    public void RemoveCollectible(GameObject collectible)
    {
        _collectedGameObjectsList.Remove(collectible);
    }

    private void ShakeCollectedGameObjects()
    {
        for (int i = _collectedGameObjectsList.Count - 1; i >= 0; i--)
        {
            _collectedGameObjectsList[i].transform.DOShakeScale(1f)
                .SetDelay(0.05f * (_collectedGameObjectsList.Count - i - 1));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money") || other.CompareTag("Gold") || other.CompareTag("Diamond"))
        {
            AddCollectible(other.gameObject);
        }
    }
}