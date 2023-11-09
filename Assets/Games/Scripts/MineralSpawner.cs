using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MineralSpawner : MonoBehaviour
{
    public UnityAction<Mineral> MineralSpawn;

    [SerializeField] private Mineral _mineral;

    private List<Mineral> _minerals = new List<Mineral>();
    private float _spawnTime;
    private bool _isSpawning;
    private WaitForSeconds waitSecond;
    private Mineral _mineralLast;


    private void Awake()
    {
        _isSpawning = true;
        _spawnTime = 1;
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        waitSecond = new WaitForSeconds(_spawnTime);

        while (_isSpawning)
        {            
            yield return waitSecond;
            CreatMineral();
        }
    }

    private void CreatMineral()
    {
        _mineralLast = Instantiate(_mineral, transform.position, Quaternion.identity);
        _minerals.Add(_mineralLast);
        _mineralLast.Destroyed += RemoveMineral;
        MineralSpawn?.Invoke(_mineralLast);
    }

    private void RemoveMineral(GameObject mineralObject)
    {
        Mineral mineral = mineralObject.GetComponent<Mineral>();

        if(mineral)
        {
            mineral.Destroyed -= RemoveMineral;
            _minerals.Remove(mineral);
        }
    }
}
