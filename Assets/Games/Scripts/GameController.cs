using UnityEngine;

public class GameController : MonoBehaviour
{
    private TowerControllBots _towerSelected;
    private bool _isTowerSelected;

    void Update()
    {
        SelectTower();
        SelectConstructionPlace();
    }

    private void SelectTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _towerSelected = hit.transform.GetComponent<TowerControllBots>();

                if (_towerSelected)
                {
                    _isTowerSelected = true;
                }
                else
                {
                    _isTowerSelected = false;
                }
            }
        }
    }

    private void SelectConstructionPlace()
    {
        if(_isTowerSelected && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    _towerSelected.SetFlagPosition(hit.point);
                }
            }            
        }
    }
}
