using UnityEngine;
using UnityEngine.Events;

public class HaudiMovement : MonoBehaviour
{
    [SerializeField] private Transform _rollEndPoint;
    [SerializeField] private Transform _runEndPoint;
    [SerializeField] private float _speed;

    public event UnityAction Exploded; 

    private bool _isRunning = false;


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
    }
}
