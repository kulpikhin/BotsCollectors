using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Vector3 _direction;

    private void Update()
    {
        Control();
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction;
    }

    private void Control()
    {
        if (Input.GetKey(KeyCode.W) && transform.position.z < DataScopes.Params._zUpScope)
        {
            _direction = new Vector3(0, 0, 1) * Time.deltaTime * _speed;
            Move(_direction);
        }

        if (Input.GetKey(KeyCode.S) && transform.position.z > DataScopes.Params._zDownScope)
        {
            _direction = new Vector3(0, 0, -1) * Time.deltaTime * _speed;
            Move(_direction);
        }

        if (Input.GetKey(KeyCode.A) && transform.position.x > DataScopes.Params._xLeftScope)
        {
            _direction = new Vector3(-1, 0, 0) * Time.deltaTime * _speed;
            Move(_direction);
        }

        if (Input.GetKey(KeyCode.D) && transform.position.x < DataScopes.Params._xRightScope)
        {
            _direction = new Vector3(1, 0, 0) * Time.deltaTime * _speed;
            Move(_direction);
        }        
    }
}
