using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CollectedCollectiblesManager : MonoBehaviour
{
    public static CollectedCollectiblesManager Instance;

    private List<CollectibleController> _collectedCollectiblesList = new List<CollectibleController>();

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
        for (int i = 0; i < _collectedCollectiblesList.Count; i++)
        {
            Vector3 target = Vector3.zero;
            if (i == 0)
            {
                target = _collectedCollectiblesList[i].transform.localPosition;
                target.x = PlayerManager.Instance.characterTransform.position.x;

                _collectedCollectiblesList[i].transform.DOLocalMove(target, 0.2f);
            }
            else
            {
                target = _collectedCollectiblesList[i].transform.localPosition;
                target.x = _collectedCollectiblesList[i - 1].transform.localPosition.x;

                _collectedCollectiblesList[i].transform.DOLocalMove(target, 0.2f);
            }
        }
    }

    private void ShakeCollectedGameObjects()
    {
        for (int i = _collectedCollectiblesList.Count - 1; i >= 0; i--)
        {
            int index = i;
            
            CollectibleController collectible = _collectedCollectiblesList[index];
            
            collectible.transform.DOScale(Vector3.one * 1.5f, 0.17f)
                .OnComplete(() =>
                {
                    collectible.transform.DOScale(Vector3.one, 0.17f);
                })
                .SetDelay(0.08f * (_collectedCollectiblesList.Count - index - 1));
        }
    }

    public void AddCollectible(CollectibleController collectible)
    {
        if (_collectedCollectiblesList.Contains(collectible))
            return;
        
        collectible.isCollected = true;
        
        collectible.transform.position = transform.position;
        collectible.transform.SetParent(transform);
            
        if (_collectedCollectiblesList.Count == 0)
        {
            collectible.transform.position = new Vector3(PlayerManager.Instance.characterTransform.position.x,
                collectible.transform.position.y,
                PlayerManager.Instance.characterTransform.position.z + 2f);
        }
        else
        {
            collectible.transform.localPosition =
                new Vector3(_collectedCollectiblesList[^1].transform.localPosition.x,
                    collectible.transform.localPosition.y,
                    _collectedCollectiblesList[^1].transform.localPosition.z + 1.2f);
        }
        
        _collectedCollectiblesList.Add(collectible);

        ShakeCollectedGameObjects();
    }

    public void RemoveCollectibleFromList(int index)
    {
        _collectedCollectiblesList[index].isCollected = false;
        _collectedCollectiblesList.RemoveAt(index);
    }

    private void ReArrangeCollectibleLine()
    {
        foreach (CollectibleController collectible in _collectedCollectiblesList)
        {
            if (_collectedCollectiblesList.Count == 0)
            {
                collectible.transform.position = new Vector3(PlayerManager.Instance.characterTransform.position.x,
                    collectible.transform.position.y,
                    PlayerManager.Instance.characterTransform.position.z + 2.5f);
            }
            else
            {
                collectible.transform.localPosition =
                    new Vector3(_collectedCollectiblesList[^1].transform.localPosition.x,
                        collectible.transform.localPosition.y,
                        _collectedCollectiblesList[^1].transform.localPosition.z + 1.6f);
            }
        }
    }

    private void CutCollectibleLine(int index)
    {
        Debug.Log("index: " + index);

        if (index == _collectedCollectiblesList.Count - 1)
        {
            RemoveCollectibleFromList(index);
        }
        else
        {
            Debug.Log("loop 1");
            for (int i = _collectedCollectiblesList.Count - 1; i > index; i--)
            {
                Debug.Log(i);
                _collectedCollectiblesList[i].transform.SetParent(null);
                DOTween.Kill(_collectedCollectiblesList[i].transform);

                Vector3 movePos = new Vector3(
                    Random.Range(PlayerManager.Instance.minX, PlayerManager.Instance.maxX),
                    _collectedCollectiblesList[i].transform.position.y,
                    _collectedCollectiblesList[i].transform.position.z + Random.Range(4f, 6f));
                _collectedCollectiblesList[i].transform.DOMove(movePos,0.5f);
            }
        
            Debug.Log("loop 2");
            for (int i = _collectedCollectiblesList.Count - 1; i >= index; i--)
            {
                Debug.Log(i);
                RemoveCollectibleFromList(i);
            }
        }
    }

    public void OnFixedObstacleTriggerred(CollectibleController collectible)
    {
        CutCollectibleLine(_collectedCollectiblesList.IndexOf(collectible));
    }

    public void OnSpinningObstacleTriggered(CollectibleController collectible)
    {
        CutCollectibleLine(_collectedCollectiblesList.IndexOf(collectible));
    }

    public void OnCardObstacleTriggered(CollectibleController collectible)
    {
        CutCollectibleLine(_collectedCollectiblesList.IndexOf(collectible));
    }

    public void OnSwingingObstacleTriggered(CollectibleController collectible)
    {
        CutCollectibleLine(_collectedCollectiblesList.IndexOf(collectible));
    }

    public void OnBarbedObstacleTriggered(CollectibleController collectible)
    {
        CutCollectibleLine(_collectedCollectiblesList.IndexOf(collectible));
    }

    public void OnUpgradeGateTriggered(CollectibleController collectible)
    {
        collectible.transform.DOScale(Vector3.one * 1.5f, 0.2f)
            .OnComplete(() => collectible.transform.DOScale(Vector3.one, 0.2f));
    }

    public void OnATMTriggered(CollectibleController collectible, GameObject atm)
    {
        int index = _collectedCollectiblesList.IndexOf(collectible);
        if(index == _collectedCollectiblesList.Count - 1)
        {
            RemoveCollectibleFromList(index);
            collectible.OnATMTriggered();
        }
        else
        {
            atm.transform.DOMoveY(-2.5f, 0.2f);
        }
    }

    public void OnPlayerHitObstacle()
    {
        for (int i = 0; i < _collectedCollectiblesList.Count; i++)
        {
            _collectedCollectiblesList[i].transform.SetParent(null);
            DOTween.Kill(_collectedCollectiblesList[i].transform);

            _collectedCollectiblesList[i].transform.DOMove(
                new Vector3(
                    Random.Range(PlayerManager.Instance.minX, PlayerManager.Instance.maxX),
                    _collectedCollectiblesList[i].transform.position.y,
                    _collectedCollectiblesList[i].transform.position.z + Random.Range(4f, 6f)),
                0.5f);
        }
        
        _collectedCollectiblesList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            AddCollectible(other.gameObject.GetComponent<CollectibleController>());
        }
    }
}