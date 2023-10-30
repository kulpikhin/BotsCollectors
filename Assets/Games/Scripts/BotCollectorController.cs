using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BotCollectorController : MonoBehaviour
{
    public event UnityAction BecomeFree;

    private BotCollectorMining _mining;
    private BotCollectorMovement _movement;
    private Mineral _mineralTarget;
    private bool _isFree;
    private bool _isBringMinerals;
    [SerializeField] private Transform _tower;

    public bool IsFree { get => _isFree; private set => _isFree = value; }

    private void Awake()
    {
        _isFree = true;
        _mining = GetComponent<BotCollectorMining>();
        _movement = GetComponent<BotCollectorMovement>();
    }

    public void TakeMineralLocation(Mineral mineralTarget)
    {
        _isFree = false;
        _mineralTarget = mineralTarget;
        DoingSwitcher(1);
    }

    private void Move(Transform target)
    {        
        _movement.StartMove(target.transform);
    }

    private void DoingSwitcher(int command)
    {
        const int CommandFindMineral = 1;
        const int CommandMining = 2;
        const int CommandComeBack = 3;

        switch (command)
        {
            case CommandFindMineral:
                Move(_mineralTarget.transform);
                break;
            case CommandMining:
                Mining();
                break;
            case CommandComeBack:
                ComeBack();
                break;
        }
    }

    private void ComeBack()
    {
        Move(_tower);
    }

    private void Mining()
    {        
        _mining.StartMining(_mineralTarget);
        StartCoroutine(WaitMining());
    }

    private IEnumerator WaitMining()
    {
        yield return new WaitWhile(() => _mining.IsMining);
        _isBringMinerals = true;
        DoingSwitcher(3);
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (_mineralTarget && collision.gameObject.transform == _mineralTarget.transform)
        {
            _movement.StopMove();
            DoingSwitcher(2);
        }

        if (_isBringMinerals && collision.gameObject.transform == _tower.transform)
        {
            _movement.StopMove();
            _isBringMinerals = false;
            _isFree = true;
            BecomeFree?.Invoke();
        }
    }
}
