using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private HandAnimation _hand;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxRotationX;
    [SerializeField] private float _minRotationX;
    [SerializeField] private Transform _afterKickPosition;

    public event UnityAction CameraStop;

    private bool _isAnimationStart = false;

    private void OnEnable()
    {
        _hand.AnimationStop += OnAnimationStop;
    }

    private void OnDisable()
    {
        _hand.AnimationStop -= OnAnimationStop;
    }


    private void Update()
    {
        if (_isAnimationStart)
            _camera.transform.Rotate(Vector3.left * _rotationSpeed * Time.deltaTime);

        LimitRotation();

        if (_camera.transform.localEulerAngles == new Vector3(345, 270, 0))
        {
            _isAnimationStart = false;
            this.enabled = false;
            CameraStop?.Invoke();
        }
    }
    public void OnKicked()
    {
        StartCoroutine(Kick());
    }

    private void OnAnimationStop(bool isAnimationStop)
    {
        _isAnimationStart = isAnimationStop;

    }

    private void LimitRotation()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.x = (rotation.x > 180) ? rotation.x - 360 : rotation.x;
        rotation.x = Mathf.Clamp(rotation.x, _minRotationX, _maxRotationX);

        transform.rotation = Quaternion.Euler(rotation);

    }

    private IEnumerator Kick()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < 10; i++)
        {
            transform.Translate((new Vector3(1,-1,0).normalized  * Time.deltaTime * 23), Space.World);
                yield return null;
        }
    }
}
