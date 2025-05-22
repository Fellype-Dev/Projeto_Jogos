using UnityEngine;
using TMPro;

public class TerminalAcesso : MonoBehaviour
{
    public Animator animPorta; // Porta com animação
    public TextMeshProUGUI textoInteracao;
    public TextMeshProUGUI textoErro;

    private bool jogadorPerto = false;
    private GameObject jogador;

    void Start()
    {
        textoInteracao.gameObject.SetActive(false);
        textoErro.gameObject.SetActive(false);
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory inventario = jogador.GetComponent<PlayerInventory>();
            if (inventario != null && inventario.temCartao2) // <-- Verifica o cartão correto
            {
                animPorta.SetBool("isOpen", true);
                textoInteracao.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(MostrarMensagemErro());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            jogador = other.gameObject;

            PlayerInventory inventario = jogador.GetComponent<PlayerInventory>();
            if (inventario != null && inventario.temCartao2)
            {
                textoInteracao.gameObject.SetActive(true);
            }
            else
            {
                textoErro.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            jogador = null;
            textoInteracao.gameObject.SetActive(false);
            textoErro.gameObject.SetActive(false);
        }
    }

    System.Collections.IEnumerator MostrarMensagemErro()
    {
        textoErro.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        textoErro.gameObject.SetActive(false);
    }
}
