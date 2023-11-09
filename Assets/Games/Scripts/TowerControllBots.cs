using System.Collections.Generic;
using UnityEngine;

public class TowerControllBots : MonoBehaviour
{
    [SerializeField] private List<BotCollectorController> _starterBotCollectors;
    [SerializeField] private BotCollectorController _botCollector;
    [SerializeField] private GameObject _flagPrefab;

    private GameObject _flag;
    private Vector3[] _spawnPoints;
    private Queue<Mineral> minerals;
    private List<BotCollectorController> _botCollectors;
    private int _freeBots;
    private int _mineralsStock;
    private bool _flagSetted;
    private int _botCost;
    private int _countSpawnPoints;
    private int _currentSpawnPoints;
    private int _costCreatTower;
    private TowerScaner _scaner;

    private void Awake()
    {
        _scaner = FindFirstObjectByType<TowerScaner>();
        _costCreatTower = 5;
        _countSpawnPoints = 12;
        _currentSpawnPoints = 0;
        _botCost = 3;
        _mineralsStock = 0;
        _botCollectors = new List<BotCollectorController>();
        minerals = new Queue<Mineral>();
    }

    private void OnEnable()
    {
        _scaner.AddTower(this);
        CreatSpawnPoints();
        _freeBots = 1;
        FillBots();
    }

    public void SetFlagPosition(Vector3 flagPosition)
    {
        if (_flag == null)
        {
            _flag = Instantiate(_flagPrefab, flagPosition + new Vector3(0, 1.39f, 0), Quaternion.identity);
            _flagSetted = true;
        }
        else
        {
            _flag.transform.position = flagPosition + new Vector3(0, 1.39f, 0);
        }
    }

    public void TrySendBot(Mineral mineralTarget)
    {
        if (_freeBots > 0)
            SendBotForMinerals(mineralTarget);
        else
            minerals.Enqueue(mineralTarget);
    }

    public void AddCreator(BotCollectorController botCreator)
    {
        _botCollectors.Add(botCreator);
        botCreator.BecomeFree += ReclaimFreeBot;
    }

    private void SendBotForMinerals(Mineral mineral)
    {
        BotCollectorController freeBot = GetFreeBot();

        if (freeBot)
            freeBot.TakeMineralLocation(mineral);
    }

    private BotCollectorController GetFreeBot()
    {
        for (int i = 0; i < _botCollectors.Count; i++)
        {
            if (_botCollectors[i].IsFree)
            {
                _freeBots--;
                return _botCollectors[i];
            }
        }

        return null;
    }

    private void ReclaimFreeBot()
    {
        _mineralsStock++;
        _freeBots++;

        if (_flagSetted)
        {
            CreatNewTower();
        }
        else
        {
            TryCreatBot();
        }

        if (minerals.Count > 0 && _freeBots > 0)
        {
            SendBotForMinerals(minerals.Dequeue());
        }
    }

    private void FillBots()
    {
        if (_starterBotCollectors.Count > 0)
        {
            foreach (BotCollectorController botCollector in _starterBotCollectors)
            {
                _botCollectors.Add(botCollector);
                botCollector.Tower = transform;
                botCollector.BecomeFree += ReclaimFreeBot;
            }

            _freeBots = _botCollectors.Count;
        }
    }

    private void OnDisable()
    {
        foreach (BotCollectorController botCollector in _botCollectors)
        {
            botCollector.BecomeFree -= ReclaimFreeBot;
        }
    }

    private void CreatNewTower()
    {
        if (_costCreatTower <= _mineralsStock)
        {
            BotCollectorController freeBot = GetFreeBot();
            freeBot.BuildTower(_flag);
            freeBot.BecomeFree -= ReclaimFreeBot;
            _botCollectors.Remove(freeBot);
            _mineralsStock -= _costCreatTower;
            _flagSetted = false;
        }
    }

    public void TryCreatBot()
    {
        if (_mineralsStock >= _botCost)
        {
            _mineralsStock -= _botCost;
            BotCollectorController newBot = Instantiate(_botCollector, GetSpawnPoint(), transform.rotation);
            _botCollectors.Add(newBot);
            newBot.BecomeFree += ReclaimFreeBot;
            newBot.Tower = transform;
            _freeBots++;
        }
    }

    private Vector3 GetSpawnPoint()
    {
        _currentSpawnPoints++;

        if (_currentSpawnPoints >= _countSpawnPoints)
            _currentSpawnPoints = 0;

        return _spawnPoints[_currentSpawnPoints];
    }

    private void CreatSpawnPoints()
    {
        _spawnPoints = new Vector3[_countSpawnPoints];
        float distance = 3;

        for (int i = 0; i < _countSpawnPoints; i++)
        {
            float angle = i * Mathf.PI * 2 / _countSpawnPoints;
            Vector3 point = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * distance;
            _spawnPoints[i] = new Vector3(transform.position.x, 0, transform.position.z) + point;
        }
    }
}
