using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodEnabler : MonoBehaviour
{
    [SerializeField] private GameObject _startFloor;
    [SerializeField] private GameObject _secondFloor;
    [SerializeField] private HaudiMovement _haudiMovement;

    private void OnEnable()
    {
        _haudiMovement.Exploded += OnExploded;
    }

    private void OnDisable()
    {
        _haudiMovement.Exploded -= OnExploded;
    }

    private void OnExploded()
    {
        _startFloor.gameObject.SetActive(false);
        _secondFloor.gameObject.SetActive(true);
    }
}
