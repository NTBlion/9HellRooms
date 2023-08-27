using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Li : MonoBehaviour
{
    [SerializeField] private HaudiMovement _haudiMovement;
    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        _haudiMovement.Exploded += OnExplode;
    }

    private void OnDisable()
    {
        _haudiMovement.Exploded -= OnExplode;
    }


    private void OnExplode()
    {
        _light.color = Color.black;
    }
}