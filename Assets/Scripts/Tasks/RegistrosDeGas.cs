using UnityEngine;
using TMPro;

public class RegistroDeGas : MonoBehaviour
{
    public int idRegistro = 1; // Defina 1, 2, 3, 4 para cada registro no Inspector
    public TextMeshProUGUI textoInteracao;
    public string mensagemFechar = "Pressione [E] para fechar o registro";
    public float distanciaInteracao = 3f;
    private bool jogadorPerto = false;
    private bool registroFechado = false;
    private AudioSource somGas;

    void Start()
    {
        if (textoInteracao != null)
            textoInteracao.gameObject.SetActive(false);

        somGas = GetComponent<AudioSource>();
        if (somGas != null)
            somGas.Play();
    }

    void Update()
    {
        if (registroFechado) return;

        if (jogadorPerto)
        {
            if (textoInteracao != null)
            {
                textoInteracao.gameObject.SetActive(true);
                textoInteracao.text = mensagemFechar;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                FecharRegistro();
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            if (textoInteracao != null)
                textoInteracao.gameObject.SetActive(false);
        }
    }

    bool EstaNoRange(Transform alvo)
    {
        return Vector3.Distance(transform.position, alvo.position) <= distanciaInteracao;
    }

    void FecharRegistro()
    {
        registroFechado = true;
        if (somGas != null)
            somGas.Stop();

        if (textoInteracao != null)
            textoInteracao.text = "Registro fechado!";
        Debug.Log($"Registro {idRegistro} fechado!");

        // Chama o manager para contabilizar
        if (RegistrosDeGasManager.instance != null)
            RegistrosDeGasManager.instance.RegistroFechado(idRegistro);

        // Esconde o texto após 2 segundos
        Invoke(nameof(EsconderTexto), 2f);
    }

    void EsconderTexto()
    {
        if (textoInteracao != null)
            textoInteracao.gameObject.SetActive(false);
    }
}