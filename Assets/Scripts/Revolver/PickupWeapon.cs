using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    public GameObject weaponPrefab; // Prefab do revólver
    public Transform attachPoint;   // Ponto onde a arma será posicionada no jogador
    private bool playerInRange = false;
    private GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
            Debug.Log("[PickupWeapon] Jogador entrou na área da arma.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            Debug.Log("[PickupWeapon] Jogador saiu da área da arma.");
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[PickupWeapon] Jogador pressionou E para pegar a arma.");

            Transform hand = player.transform.Find("WeaponSocket");

            if (hand == null)
            {
                Debug.LogWarning("[PickupWeapon] Não foi encontrado um objeto chamado 'WeaponSocket' no jogador!");
                return;
            }

            if (weaponPrefab == null)
            {
                Debug.LogWarning("[PickupWeapon] Nenhum prefab de arma foi atribuído.");
                return;
            }

            Instantiate(weaponPrefab, hand.position, hand.rotation, hand);
            Debug.Log("[PickupWeapon] Arma instanciada no jogador.");

            Destroy(gameObject);
            Debug.Log("[PickupWeapon] Arma no chão destruída após ser pega.");
        }
    }
}
