using UnityEngine;

public class HaudiSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _haudi;
    [SerializeField] private CameraBehavior _cameraBehavior;
    [SerializeField] private Transform _spawnPosition;


    private void OnEnable()
    {
        _cameraBehavior.CameraStop += OnCameraStop;
    }

    private void OnDisable()
    {
        _cameraBehavior.CameraStop -= OnCameraStop;
    }

    private void OnCameraStop()
    {
        Instantiate(_haudi, transform.position, Quaternion.Euler(0,90,0));
    }
}
