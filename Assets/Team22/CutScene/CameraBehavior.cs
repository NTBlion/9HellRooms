using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private HandAnimation _hand;
    [SerializeField] private float _endRotationX;
    [SerializeField] private Camera _camera;

    private float _startRotationX;

    private void OnEnable()
    {
        _hand.AnimationStop += OnAnimationStop;
        _startRotationX = transform.localRotation.x;
    }

    private void OnDisable()
    {
        _hand.AnimationStop -= OnAnimationStop;
    }

    private void OnAnimationStop(bool isAnimationStop)
    {
        _camera.transform.Rotate(new Vector3(_startRotationX, 0, 0));
    }
}
