using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAnimation : MonoBehaviour
{
    [SerializeField] private CameraBehavior _camera;
    private Animator _animator;

    private bool _isKicked = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("kick") && _isKicked == false)
        {
            _camera.OnKicked();
            _isKicked = true;
        }
    }
}
