using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BotCollectorMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Transform _target;
    private Animator _animator;
    private bool _isMoving;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartMove(Transform target)
    {
        _target = target.transform;
        _isMoving = true;
        _animator.SetBool(BotsAnimatorData.Params.IsMove, _isMoving);
        StartCoroutine(Move());
    }

    public void StopMove()
    {
        _isMoving = false;
        _animator.SetBool(BotsAnimatorData.Params.IsMove, _isMoving);
        StopCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (_isMoving)
        {
            transform.LookAt(_target);
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
