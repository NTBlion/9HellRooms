using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPointBigBox;
    [SerializeField] private Transform _prefabBigBox;
    [SerializeField] private Transform _boxPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField, Min(1)] private float _spawnTime = 2f;
    [SerializeField, Min(1)] private float _trust = 2f;


    private int _boxesDestroyed;
    private int _maxSpawnedBoxed;
    private List<Transform> _boxList = new List<Transform>();

    public event EventHandler AllBoxesDestroyed;
    private void Start()
    {
        _boxesDestroyed = 0;
        _maxSpawnedBoxed = 7;
        Spawn();
    }

    private void Spawn()
    {
        StartCoroutine(SpawnBoxes());
    }

    private IEnumerator SpawnBoxes()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            Transform box = Instantiate(_boxPrefab, _spawnPoints[i].position, Quaternion.identity);
            if(box.gameObject.TryGetComponent(out Rigidbody rigidbody))
                rigidbody.AddForce(UnityEngine.Random.insideUnitSphere * _trust, ForceMode.Impulse);
            if(box.gameObject.TryGetComponent(out BoxController boxController))
                boxController.Init(UpDestroyCounter);
            yield return new WaitForSeconds(_spawnTime);
        }
        
        Transform bigBox = Instantiate(_boxPrefab, _spawnPointBigBox.position, Quaternion.identity);
        if(bigBox.gameObject.TryGetComponent(out BoxController bigBoxController))
            bigBoxController.Init(UpDestroyCounter);
    }

    public void UpDestroyCounter()
    {
        _boxesDestroyed++;
        
        if(_boxesDestroyed == _maxSpawnedBoxed)
            AllBoxesDestroyed?.Invoke(this, EventArgs.Empty);
    }
}
