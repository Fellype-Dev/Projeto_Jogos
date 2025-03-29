using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public PlayerStats playerStats;

    void Start()
    {
        playerStats.ResetHealth(); 
    }

    public void TakeDamage(int damage)
    {
        playerStats.TakeDamage(damage);
    }
}