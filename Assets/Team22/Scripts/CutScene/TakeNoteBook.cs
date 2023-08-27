using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeNoteBook : MonoBehaviour
{
    [SerializeField] private GameObject _notebook;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _table;
    [SerializeField] private GameObject _effect;
    [SerializeField] private Vector3 _spawnPoint;
    private bool _isPick;

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("pick") && _isPick == false)
        {
            _notebook.gameObject.SetActive(false);
            _table.gameObject.SetActive(false);
            Instantiate(_effect, _spawnPoint, Quaternion.identity);
            _isPick = true;
        }
    }
}
