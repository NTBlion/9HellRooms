using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsEnabler : MonoBehaviour
{
    [SerializeField] private HaudiMovement _haudiMovement;
    [SerializeField] private HandAnimation _leftHand;
    [SerializeField] private HandAnimation _rightHand;


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
        StartCoroutine(RemoveHands());
    }

    private IEnumerator RemoveHands()
    {
        yield return new WaitForSeconds(3);
        _leftHand.gameObject.SetActive(false);
        _rightHand.gameObject.SetActive(false);
    }
}
