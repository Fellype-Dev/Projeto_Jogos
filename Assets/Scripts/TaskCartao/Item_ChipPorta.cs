using UnityEngine;
using TMPro;

public class Item_ChipPorta : MonoBehaviour
{
    private TextMeshProUGUI textoColetar;
    private bool podeColetar = false;

    void Start()
    {
        // Busca automaticamente o texto pelo nome (certifique-se de que existe!)
        textoColetar = GameObject.Find("Texto_Coletar").GetComponent<TextMeshProUGUI>();
        textoColetar.gameObject.SetActive(false);
    }

    void Update()
    {
        // Só permite coletar se estiver dentro do trigger
        if (podeColetar && Input.GetKeyDown(KeyCode.E))
        {
            Coletar();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            podeColetar = true;
            if (textoColetar != null)
                textoColetar.gameObject.SetActive(true); // Mostra o texto
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            podeColetar = false;
            if (textoColetar != null)
                textoColetar.gameObject.SetActive(false); // Esconde o texto
        }
    }

    void Coletar()
    {
        if (textoColetar != null)
            textoColetar.gameObject.SetActive(false); // Esconde ao coletar
        
        Destroy(gameObject); // Remove o chip
    }
}