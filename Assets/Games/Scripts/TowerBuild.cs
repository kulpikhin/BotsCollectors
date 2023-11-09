using System.Collections;
using UnityEngine;

public class TowerBuild : MonoBehaviour
{
    private float _secondsBuildDuration = 4;
    private Vector3 _startPoint;
    private Vector3 _targetPoint;
    private TowerControllBots _towerController;

    public float SecondsBuildDuration { get => _secondsBuildDuration; private set => _secondsBuildDuration = value; }

    private void OnEnable()
    {
        _towerController = GetComponent<TowerControllBots>();
        _startPoint = transform.position;
        _targetPoint = new Vector3(_startPoint.x, 0, _startPoint.z);
    }

    public void GetCreator(GameObject botCreator)
    {
        BotCollectorController botCreatorController = botCreator.GetComponent<BotCollectorController>();
        _towerController.AddCreator(botCreatorController);
    }

    public void Construction()
    {
        StartCoroutine(StartConstruction());
    }

    private IEnumerator StartConstruction()
    {
        float elapsedTime = 0;

        while (_secondsBuildDuration != elapsedTime)
        {
            transform.position = Vector3.Lerp(_startPoint, _targetPoint, elapsedTime/_secondsBuildDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
