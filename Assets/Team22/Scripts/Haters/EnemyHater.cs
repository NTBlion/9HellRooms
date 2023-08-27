using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHater : MonoBehaviour
{
    [SerializeField] private PlayerHater _player;


    private Vector3 _enemyPosition;
    private NavMeshAgent _agent;

    private bool _isStop = true;

    private float _distanceBetweenPlayer = 50;

    private void Awake()
    {
        _enemyPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Vector3.Distance(_enemyPosition, _player.transform.position) < _distanceBetweenPlayer)
        {
            _agent.SetDestination(_player.transform.position);
            _isStop = false;
        }
    }
}
