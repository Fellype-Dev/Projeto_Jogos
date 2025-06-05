using UnityEngine;

public class Acido : MonoBehaviour
{
    public float danoPorSegundo = 10f;

    void OnTriggerStay(Collider other)
    {
        PlayerCharacterController player = other.GetComponent<PlayerCharacterController>();
        if (player != null)
        {
            player.ReceberDano(danoPorSegundo * Time.deltaTime);
        }
    }
}
