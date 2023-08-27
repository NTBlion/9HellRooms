using System;
using UnityEngine;

public class FloorDestroyer : MonoBehaviour
{
    [SerializeField] private Transform _floorTransform;
    [SerializeField] private BoxSpawner _boxSpawner;

    private void Start()
    {
        _boxSpawner.AllBoxesDestroyed += BoxSpawnerOnAllBoxesDestroyed;
    }

    private void BoxSpawnerOnAllBoxesDestroyed(object sender, EventArgs e)
    {
        DestroyFloor();
    }

    private void DestroyFloor()
    {
        Debug.Log("Destroy floor");
        _floorTransform.gameObject.SetActive(false);
    }
}
