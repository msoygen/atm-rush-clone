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

    private List<GameObject> _collectedCollectiblesList = new List<GameObject>();

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

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, characterTransform.position.z);
    }

    public void SwerveFollow()
    {
        Vector3 target = Vector3.zero;
        for (int i = 0; i < _collectedCollectiblesList.Count; i++)
        {
            if (i == 0)
            {
                target = _collectedCollectiblesList[i].transform.localPosition;
                target.x = characterTransform.position.x;

                _collectedCollectiblesList[i].transform.DOLocalMove(target, 0.3f);
            }
            else
            {
                target = _collectedCollectiblesList[i].transform.localPosition;
                target.x = _collectedCollectiblesList[i - 1].transform.localPosition.x;

                _collectedCollectiblesList[i].transform.DOLocalMove(target, 0.3f);
            }
        }
    }

    public void AddCollectible(GameObject collectible)
    {
        if (_collectedCollectiblesList.Contains(collectible))
            return;
        
        collectible.transform.SetParent(transform);
        if (_collectedCollectiblesList.Count == 0)
        {
            collectible.transform.position = new Vector3(characterTransform.position.x,
                collectible.transform.position.y,
                characterTransform.position.z + 2.5f);
        }
        else
        {
            collectible.transform.localPosition =
                new Vector3(_collectedCollectiblesList[^1].transform.localPosition.x,
                    collectible.transform.localPosition.y,
                    _collectedCollectiblesList[^1].transform.localPosition.z + 1.6f);
        }

        _collectedCollectiblesList.Add(collectible);

        collectible.AddComponent<CollectibleController>();

        ShakeCollectedGameObjects();
    }

    public void RemoveCollectibleFromList(GameObject collectible)
    {
        Destroy(collectible.GetComponent<CollectibleController>());
        _collectedCollectiblesList.Remove(collectible);
    }

    public void RemoveCollectibleFromList(int index)
    {
        Destroy(_collectedCollectiblesList[index].GetComponent<CollectibleController>());
        _collectedCollectiblesList.RemoveAt(index);
    }

    private void ShakeCollectedGameObjects()
    {
        for (int i = _collectedCollectiblesList.Count - 1; i >= 0; i--)
        {
            int index = i;
            _collectedCollectiblesList[index].transform.DOScale(Vector3.one * 1.5f, 0.2f)
                .OnComplete(() => _collectedCollectiblesList[index].transform.DOScale(Vector3.one, 0.2f))
                .SetDelay(0.05f * (_collectedCollectiblesList.Count - index - 1));
        }
    }

    public void OnFixedObstacleTriggerred(GameObject collectible)
    {
        int index = _collectedCollectiblesList.IndexOf(collectible);

        for (int i = index + 1; i < _collectedCollectiblesList.Count; i++)
        {
            _collectedCollectiblesList[i].transform.SetParent(null);

            _collectedCollectiblesList[i].transform.position += Vector3.forward * 4;
        }

        for (int i = _collectedCollectiblesList.Count - index; i > 0; i--)
        {
            RemoveCollectibleFromList(_collectedCollectiblesList.Count-1);
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