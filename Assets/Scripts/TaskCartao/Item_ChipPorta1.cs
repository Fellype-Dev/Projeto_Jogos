using UnityEngine;
using TMPro;

public class Item_ChipPorta1 : MonoBehaviour
{
    private TextMeshProUGUI textoColetar;
    private bool podeColetar = false;

    void Start()
    {
        GameObject objTexto = GameObject.Find("Texto_Coletar");
        if (objTexto != null)
        {
            textoColetar = objTexto.GetComponent<TextMeshProUGUI>();
            textoColetar.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Texto_Coletar não encontrado!");
        }
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
        if (other.CompareTag("Player") && textoColetar != null)
        {
            podeColetar = true;
            textoColetar.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && textoColetar != null)
        {
            podeColetar = false;
            textoColetar.gameObject.SetActive(false);
        }
    }

    void Coletar()
    {
        textoColetar.gameObject.SetActive(false);

        PlayerInventory inventario = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerInventory>();
        if (inventario != null)
            inventario.temCartao1 = true;

        Destroy(gameObject);
    }
}