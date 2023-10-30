using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCollectorMining : MonoBehaviour
{
    private Animator _animator;
    private WaitForSeconds _waitSeconds;
    private Mineral _mineralTarget;
    private float _oneTickDuration;
    private bool _isMining;

    public bool IsMining { get => _isMining; private set => _isMining = value; }

    private void Awake()
    {
        _oneTickDuration = 1.5f;
        _waitSeconds = new WaitForSeconds(_oneTickDuration);
        _animator = GetComponent<Animator>();
    }

    public void StartMining(Mineral mineral)
    {
        _mineralTarget = mineral;
        _isMining = true;
        StartCoroutine(Mining());
    }

    private IEnumerator Mining()
    {
        _animator.SetBool(BotsAnimatorData.Params.IsMining, _isMining);

        while (_isMining)
        {
            if (_mineralTarget)
            {
                _mineralTarget.TakeDamage();

                if (_mineralTarget)
                {
                    yield return _waitSeconds;
                }    

            }
            else
            {
                _isMining = false;
                _animator.SetBool(BotsAnimatorData.Params.IsMining, _isMining);
                StopCoroutine(Mining());
            }
        }
    }
}
