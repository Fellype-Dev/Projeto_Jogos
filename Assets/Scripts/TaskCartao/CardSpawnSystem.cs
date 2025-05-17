using UnityEngine;

public class ChipSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject chipPrefab;
    public Transform spawnPoint;

    void Start()
    {
        SpawnChip(); // Chama apenas uma vez quando o jogo inicia
    }

    void SpawnChip()
    {
        if(chipPrefab && spawnPoint)
        {
            GameObject newChip = Instantiate(chipPrefab, 
                spawnPoint.position, 
                Random.rotation);
            
            Rigidbody rb = newChip.GetComponent<Rigidbody>();
            if(rb) rb.velocity = Vector3.zero;
        }
    }
}