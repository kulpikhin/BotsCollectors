using UnityEngine;
using UnityEngine.Events;

public class Mineral : MonoBehaviour
{
    public UnityAction<GameObject> Destroyed;

    private int _health;
    private int _currentHealth;

    private void Start()
    {
        _health = 3;
        _currentHealth = _health;
    }

    private void TryDestroy()
    {
        if (_currentHealth <= 0)
        {
            Destroyed?.Invoke(this.gameObject);
            Destroy(gameObject, 1.2f);
        }
    }

    public void TakeDamage()
    {
        _currentHealth--;
        TryDestroy();
    }
}

