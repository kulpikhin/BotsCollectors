using System.Collections.Generic;
using UnityEngine;

public class TowerScaner : MonoBehaviour
{
    private MineralSpawner _mineralSpawner;
    private TowerControllBots _towerBotController;
    public List<TowerControllBots> _towers;
    public int _currentBuildIndex;

    private void Awake()
    {
        _currentBuildIndex = 0;
        _towers = new List<TowerControllBots>();
    }

    private void OnEnable()
    {
        _towerBotController = GetComponent<TowerControllBots>();
        _mineralSpawner = FindAnyObjectByType<MineralSpawner>();
        _mineralSpawner.MineralSpawn += SelectTower;
    }

    private void OnDisable()
    {
        _mineralSpawner.MineralSpawn -= SelectTower;
    }

    public void AddTower(TowerControllBots tower)
    {
        _towers.Add(tower);
    }

    private void SelectTower(Mineral mineral)
    {
        _towers[_currentBuildIndex].TrySendBot(mineral);
        _currentBuildIndex++;

        if (_currentBuildIndex >= _towers.Count)
            _currentBuildIndex = 0;
    }
}
