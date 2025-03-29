using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int maxHealth = 100;
    public int currentHealth;

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        Debug.Log("Ta tomando dano burrão, sua vida atual é: " + currentHealth);
    }
}