using UnityEngine;
using TMPro;

public class TaskManutencao : MonoBehaviour
{
    public string mensagemInteracao = "Pressione [E] para realizar a manutenção";
    public string mensagemEmAndamento = "Realizando manutenção...";
    public string mensagemConcluida = "Manutenção concluída!";
    public float distanciaInteracao = 3f;
    public TextMeshProUGUI textoInteracao;
    public float tempoManutencao = 4f; // Tempo em segundos

    private bool jogadorPerto = false;
    private bool manutencaoEmAndamento = false;
    private bool manutencaoFeita = false;
    private GameObject jogador;

    void Start()
    {
        if (textoInteracao != null)
            textoInteracao.gameObject.SetActive(false);
    }

    void Update()
    {
        if (manutencaoEmAndamento || manutencaoFeita) return;

        if (jogadorPerto)
        {
            if (textoInteracao != null)
            {
                textoInteracao.gameObject.SetActive(true);
                textoInteracao.text = mensagemInteracao;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(ExecutarManutencao());
            }
        }
        else
        {
            if (textoInteracao != null)
                textoInteracao.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && EstaNoRange(other.transform))
        {
            jogadorPerto = true;
            jogador = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            jogador = null;
            if (textoInteracao != null)
                textoInteracao.gameObject.SetActive(false);
        }
    }

    bool EstaNoRange(Transform alvo)
    {
        return Vector3.Distance(transform.position, alvo.position) <= distanciaInteracao;
    }

    System.Collections.IEnumerator ExecutarManutencao()
    {
        manutencaoEmAndamento = true;
        if (textoInteracao != null)
            textoInteracao.text = mensagemEmAndamento;

        // Espera o tempo de manutenção
        yield return new WaitForSeconds(tempoManutencao);

        manutencaoEmAndamento = false;
        manutencaoFeita = true;
        if (textoInteracao != null)
            textoInteracao.text = mensagemConcluida;

        Debug.Log("Manutenção realizada com sucesso!");

        // Esconde o texto após 2 segundos
        yield return new WaitForSeconds(2f);
        if (textoInteracao != null)
            textoInteracao.gameObject.SetActive(false);
    }
}