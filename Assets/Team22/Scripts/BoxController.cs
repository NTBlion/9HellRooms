using System;
using Unity.FPS.Game;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    [SerializeField] private Health _health;

    private Action _onDestroy;
    private void Start()
    {
        _health.OnDie += OnDie;
    }

    public void Init(Action onDestroy)
    {
        _onDestroy = onDestroy;
    }

    private void OnDie()
    {
        _onDestroy();
        Destroy(gameObject);
    }
}
