using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BotCollectorController : MonoBehaviour
{
    [SerializeField] private GameObject _towerPrefab;

    public event UnityAction BecomeFree;

    private Animator _animator;
    private Transform _myTower;
    private BotCollectorMining _mining;
    private BotCollectorMovement _movement;
    private Mineral _mineralTarget;
    private bool _isFree;
    private bool _isBringMinerals;
    private bool _isBuilding;
    private WaitForSeconds _waitConstructing;
    private GameObject _flag;

    public Transform Tower { get => _myTower; set => _myTower = value; }
    public bool IsFree { get => _isFree; private set => _isFree = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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

    private void Move(Vector3 target)
    {
        _movement.StartMove(target);
    }

    private void DoingSwitcher(int command)
    {
        const int CommandFindMineral = 1;
        const int CommandMining = 2;
        const int CommandComeBack = 3;

        switch (command)
        {
            case CommandFindMineral:
                Move(_mineralTarget.transform.position);
                break;
            case CommandMining:
                Mining();
                break;
            case CommandComeBack:
                ComeBack();
                break;
        }
    }

    public void BuildTower(GameObject flag)
    {
        _flag = flag;
        Move(_flag.transform.position - new Vector3(0, 1.39f, 0));
        _isBuilding = true;
    }

    public void StartConstruct()
    {
        GameObject tower = Instantiate(_towerPrefab, new Vector3(_flag.transform.position.x, 1.9f, _flag.transform.position.z) - new Vector3(0, 4, 0), Quaternion.identity);
        TowerBuild builder = tower.GetComponent<TowerBuild>();
        builder.Construction();
        builder.GetCreator(gameObject);
        _waitConstructing = new WaitForSeconds(builder.SecondsBuildDuration);
        _myTower = tower.transform;
        StartCoroutine(Constructing());
    }

    private IEnumerator Constructing()
    {
        _isFree = false;
        _movement.StopMove();
        _animator.SetBool(BotsAnimatorData.Params.IsMining, _isBuilding);
        yield return _waitConstructing;
        _isFree = true;
        _isBuilding = false;
        _animator.SetBool(BotsAnimatorData.Params.IsMining, _isBuilding);
        Destroy(_flag);
    }

    private void ComeBack()
    {
        Move(_myTower.transform.position);
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

        if (_isBringMinerals && collision.gameObject.transform == _myTower.transform)
        {
            _movement.StopMove();
            _isBringMinerals = false;
            _isFree = true;
            BecomeFree?.Invoke();
        }

        if(_isBuilding && collision.gameObject.transform == _flag.gameObject.transform)
        {
            StartConstruct();
        }
    }
}
