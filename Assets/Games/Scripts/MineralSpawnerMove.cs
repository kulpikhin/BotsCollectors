using UnityEngine;

public class MineralSpawnerMove : MonoBehaviour
{
    private bool _isFreePlace;

    private void Start()
    {
        _isFreePlace = true;
        transform.position = GeneratePosition();
    }

    private void Update()
    {
        FindFreePlace();
    }

    private Vector3 GeneratePosition()
    {
        float randomX = Random.Range(DataScopes.Params._xLeftScope, DataScopes.Params._xRightScope);
        float randomZ = Random.Range(DataScopes.Params._zDownScope, DataScopes.Params._zUpScope);
        return new Vector3(randomX, 0, randomZ);
    }

    private void FindFreePlace()
    {
        if (_isFreePlace == false)
            transform.position = GeneratePosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        _isFreePlace = false;
    }

    private void OnTriggerExit(Collider collision)
    {
        _isFreePlace = true;
    }
}
