using UnityEngine;
using TMPro;

public class Item_Cartao : MonoBehaviour
{
    private TextMeshProUGUI textoColetar;
    private bool podeColetar = false;

    void Start()
    {
        textoColetar = GameObject.Find("Texto_Coletar").GetComponent<TextMeshProUGUI>();
        textoColetar.gameObject.SetActive(false);
    }

    void Update()
    {
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
            textoColetar.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            podeColetar = false;
            textoColetar.gameObject.SetActive(false);
        }
    }

    void Coletar()
    {
        textoColetar.gameObject.SetActive(false);

        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
        {
            PlayerInventory inventario = jogador.GetComponent<PlayerInventory>();
            if (inventario != null)
            {
                inventario.temCartao = true;
            }
        }

        Destroy(gameObject);
    }
}
