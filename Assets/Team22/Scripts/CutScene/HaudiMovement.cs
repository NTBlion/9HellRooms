using UnityEngine;
using UnityEngine.Events;

public class HaudiMovement : MonoBehaviour
{
    [SerializeField] private Transform _rollEndPoint;
    [SerializeField] private Transform _runEndPoint;
    [SerializeField] private float _speed;
    [SerializeField] private CameraBehavior _cameraBehavior;

    public event UnityAction Exploded; 

    private bool _isRunning = false;
    private bool _isGettingUp= false;

    private void OnEnable()
    {
        _cameraBehavior.GetUp += OnGettingUp;
    }

    private void OnDisable()
    {
        _cameraBehavior.GetUp -= OnGettingUp;
    }


    private void Update()
    {
        if (!_isRunning)
        {
            transform.Translate(Vector3.right * Time.deltaTime * _speed);
        }

        if (transform.position.x >= _rollEndPoint.position.x)
        {
            _isRunning = true;
            transform.Translate(Vector3.right * Time.deltaTime * _speed);
        }

        if (transform.position.x >= _runEndPoint.position.x)
        {
            transform.position = _runEndPoint.position;
            Exploded?.Invoke();
        }

        if (_isGettingUp)
        {
            _cameraBehavior.transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * 5, Space.World);
        }
    }

    private void OnGettingUp()
    {
        _cameraBehavior.transform.position = new Vector3(_cameraBehavior.transform.position.x, 1.1f, 0);
        _isGettingUp = true;
    }
}
