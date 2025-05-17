using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class TerminalAcesso : MonoBehaviour
{
    public Animator animPorta;
    public BoxCollider portaCollider;
    public NavMeshObstacle portaObstacle; // <- Adicione este novo campo
    public TextMeshProUGUI textoInteracao;
    public TextMeshProUGUI textoErro;

    private bool jogadorPerto = false;
    private GameObject jogador;

    void Start()
    {
        textoInteracao.gameObject.SetActive(false);
        textoErro.gameObject.SetActive(false);
        AtualizarEstadoPorta();
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory inventario = jogador.GetComponent<PlayerInventory>();
            if (inventario != null && inventario.temCartao)
            {
                animPorta.SetBool("isOpen", true);
                textoInteracao.gameObject.SetActive(false);
                AtualizarEstadoPorta();
            }
            else
            {
                StartCoroutine(MostrarMensagemErro());
            }
        }

        // Garante que sempre esteja com o estado certo
        AtualizarEstadoPorta();
    }

    void AtualizarEstadoPorta()
    {
        bool portaAberta = animPorta.GetBool("isOpen");

        if (portaCollider != null)
            portaCollider.enabled = !portaAberta;

        if (portaObstacle != null)
            portaObstacle.enabled = !portaAberta;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            jogador = other.gameObject;

            PlayerInventory inventario = jogador.GetComponent<PlayerInventory>();
            if (inventario != null && inventario.temCartao)
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
