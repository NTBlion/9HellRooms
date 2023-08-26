using UnityEngine;

public class OfficeExplode : MonoBehaviour
{
    [SerializeField] private HaudiMovement _haudiMovement;

    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        _haudiMovement.Exploded += OnExploded;
    }

    private void OnDisable()
    {
        _haudiMovement.Exploded -= OnExploded;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnExploded()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.AddExplosionForce(1f,Vector3.up * 3,6f,1,ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90)));
    }
}