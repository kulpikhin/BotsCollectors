using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerControllBots : MonoBehaviour
{
    [SerializeField] private List<BotCollectorController> _starterBotCollectors;

    private Queue<Mineral> minerals;
    private List<BotCollectorController> _botCollectors;
    private TowerScaner _scaner;
    private int _freeBots;
    private int _mineralsStock;

    private void Awake()
    {
        _mineralsStock = 0;
        _botCollectors = new List<BotCollectorController>();
        minerals = new Queue<Mineral>();
        _scaner = GetComponent<TowerScaner>();
        FillBots();
        _freeBots = _botCollectors.Count;
    }

    public void TrySendBot(Mineral mineralTarget)
    {
        if (_freeBots > 0)
            FindBot(mineralTarget);
        else
            minerals.Enqueue(mineralTarget);
    }

    private void FindBot(Mineral mineral)
    {

        for (int i = 0; i < _botCollectors.Count; i++)
        {
            if (_botCollectors[i].IsFree)
            {
                _botCollectors[i].TakeMineralLocation(mineral);
                _freeBots--;
                return;
            }
        }
    }

    private void ReclaimFreeBot()
    {
        _mineralsStock++;
        _freeBots++;

        if(minerals.Count > 0)
        {
            FindBot(minerals.Dequeue());
        }
    }

    private void FillBots()
    {
        if (_starterBotCollectors != null)
        {
            foreach (BotCollectorController botCollector in _starterBotCollectors)
            {
                _botCollectors.Add(botCollector);
                botCollector.BecomeFree += ReclaimFreeBot;
            }
        }
    }

    private void OnDisable()
    {
        foreach (BotCollectorController botCollector in _botCollectors)
        {
            botCollector.BecomeFree -= ReclaimFreeBot;
        }
    }
}
