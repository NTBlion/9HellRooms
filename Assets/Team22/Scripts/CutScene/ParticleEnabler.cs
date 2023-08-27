using UnityEngine;

public class ParticleEnabler : MonoBehaviour
{
    [SerializeField] private HaudiMovement _haudiMovement;
    [SerializeField] private ParticleTemplate _particleTemplate;

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
        _particleTemplate.gameObject.SetActive(true);
    }
}
