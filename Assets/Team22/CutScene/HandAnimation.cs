using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class HandAnimation : MonoBehaviour
{
    [SerializeField] private int _stopAnimationDelay = 3;
    private Animator _animator;

    public event UnityAction<bool> AnimationStop;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(StopAnimation());
    }

    private IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(_stopAnimationDelay);
        _animator.enabled = false;
        AnimationStop?.Invoke(true);
    }
}
