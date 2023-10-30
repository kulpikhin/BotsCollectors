using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerScaner : MonoBehaviour
{
    [SerializeField] private MineralSpawner _mineralSpawner;

    private TowerControllBots _botController;

    private void Awake()
    {
        _botController = GetComponent<TowerControllBots>();
    }

    private void SetMineralTarget(Mineral mineral)
    {
        _botController.TrySendBot(mineral);
    }

    private void OnEnable()
    {
        _mineralSpawner.MineralSpawn += SetMineralTarget;
    }

    private void OnDisable()
    {
        _mineralSpawner.MineralSpawn -= SetMineralTarget;
    }
}
