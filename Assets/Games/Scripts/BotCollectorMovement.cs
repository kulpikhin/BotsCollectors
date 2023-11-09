using System.Collections;
using UnityEngine;

public class BotCollectorMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector3 _target;
    private Animator _animator;
    private bool _isMoving;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartMove(Vector3 target)
    {
        _target = target;
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
            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
