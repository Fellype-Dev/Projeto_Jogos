using UnityEngine;

public class AcidPuddle : MonoBehaviour
{
    public int damagePerSecond = 10;
    private float damageCounter = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                damageCounter += damagePerSecond * Time.deltaTime;


                if (damageCounter >= 1f)
                {
                    int damageToApply = Mathf.FloorToInt(damageCounter);
                    playerHealth.TakeDamage(damageToApply);
                    damageCounter -= damageToApply; 

                    Debug.Log("Player took damage! Current health: " + playerHealth.playerStats.currentHealth);
                }
            }
        }
    }
}