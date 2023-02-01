using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectedCollectiblesManager : MonoBehaviour
{
    public static CollectedCollectiblesManager Instance;

    [SerializeField] private Transform characterTransform;

    private List<GameObject> _collectedGameObjectsList = new List<GameObject>();

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
                characterTransform.position.z + 1.3f);
        }
        else
        {
            collectible.transform.localPosition =
                new Vector3(_collectedGameObjectsList[^1].transform.localPosition.x,
                    collectible.transform.localPosition.y,
                    _collectedGameObjectsList[^1].transform.localPosition.z + 1.1f);
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
            int index = i;
            _collectedGameObjectsList[index].transform.DOScale(Vector3.one * 1.5f, 0.2f)
                .OnComplete(() => _collectedGameObjectsList[index].transform.DOScale(Vector3.one, 0.2f))
                .SetDelay(0.05f * (_collectedGameObjectsList.Count - index - 1));
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