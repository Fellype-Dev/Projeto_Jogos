using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public AudioSource footstepAudio;  // Referência ao AudioSource que vai tocar o som dos passos
    public AudioClip footstepSound;    // Som dos passos

    private Animator animator;
    private bool isWalking = false;
    private bool isRunning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Checando se o personagem está andando ou correndo
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Verifica se a tecla Shift está pressionada para determinar se o personagem está correndo
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Se o personagem estiver se movendo (andando ou correndo)
        if (horizontal != 0 || vertical != 0)
        {
            if (!isWalking)
            {
                // Começar o som dos passos
                footstepAudio.clip = footstepSound;
                footstepAudio.loop = true;  // Para o som ficar em loop enquanto ele andar
                footstepAudio.Play();
                isWalking = true;
            }

            // Aumentar a velocidade do som se o personagem estiver correndo
            if (isRunning)
            {
                footstepAudio.pitch = 1.5f;  // Aumenta a velocidade do som (exemplo: 1.5x a velocidade normal)
            }
            else
            {
                footstepAudio.pitch = 1.0f;  // Velocidade normal do som
            }
        }
        else
        {
            if (isWalking)
            {
                // Parar o som dos passos
                footstepAudio.Stop();
                isWalking = false;
            }
        }

        // Adicione sua lógica de animação de movimento aqui (ex: correr ou andar)
        // animator.SetFloat("Speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }
}
