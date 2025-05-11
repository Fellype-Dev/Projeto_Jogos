using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;
    public float runSpeed = 8f;
    public float gravity = -45.0f;
    public float jumpHeight = 2f;
    private float yVelocity;
    private bool isGrounded;

    [Header("Estamina")]
    public float estaminaMaxima = 100f;
    public float estaminaAtual;
    public float custoCorrida = 20f;
    public float regenEstamina = 15f;
    private bool estaminaEsgotada = false;

    [Header("Vida")]
    public float vidaMaxima = 100f;
    public float vidaAtual;

    [Header("Referências")]
    public CharacterController cc;
    public Animator animator;
    public Transform cameraTransform;

    [Header("HUD")]
    public Image barraVida;
    public Image barraEstamina;

    [Header("Inventário")]
    public string[] inventario = new string[2];  // Capacidade de 2 itens
    private int inventarioIndex = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        estaminaAtual = estaminaMaxima;
        vidaAtual = vidaMaxima;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleCameraRotation();
        UpdateAnimations();
        AtualizarHUD();
        HandleInventoryInteraction();
    }

    void HandleMovement()
    {
        isGrounded = cc.isGrounded;

        if (isGrounded && yVelocity < 0)
            yVelocity = -2f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool querCorrer = Input.GetKey(KeyCode.LeftShift) && !estaminaEsgotada;
        float currentSpeed = querCorrer ? runSpeed : speed;

        if (querCorrer && (horizontal != 0 || vertical != 0))
        {
            if (!estaminaEsgotada)
            {
                estaminaAtual -= custoCorrida * Time.deltaTime;
                estaminaAtual = Mathf.Max(estaminaAtual, 0f);

                if (estaminaAtual <= 0f)
                    estaminaEsgotada = true;
            }
            else
            {
                // Perder vida ao tentar correr sem estamina
                vidaAtual -= 10f * Time.deltaTime;
                vidaAtual = Mathf.Max(vidaAtual, 0f);
            }
        }
        else
        {
            estaminaAtual += regenEstamina * Time.deltaTime;
            estaminaAtual = Mathf.Min(estaminaAtual, estaminaMaxima);

            if (estaminaAtual >= estaminaMaxima)
                estaminaEsgotada = false;
        }

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        cc.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (animator != null)
                animator.SetTrigger("Jump");
        }

        yVelocity += gravity * Time.deltaTime;
        cc.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(
                cameraTransform.localEulerAngles.x,
                0f,
                0f
            );
        }
    }

    void UpdateAnimations()
    {
        if (animator == null) return;

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("IsWalking", isMoving);
        animator.SetBool("IsGrounded", isGrounded);
    }

    void AtualizarHUD()
    {
        if (barraVida != null)
            barraVida.fillAmount = vidaAtual / vidaMaxima;

        if (barraEstamina != null)
            barraEstamina.fillAmount = estaminaAtual / estaminaMaxima;
    }

    void HandleInventoryInteraction()
    {
        if (Input.GetKeyDown(KeyCode.I))  // Pressione 'I' para interagir com o inventário
        {
            // Tenta pegar um novo item (apenas para exemplo, você pode usar colisões, pickups, etc)
            if (inventarioIndex < inventario.Length)
            {
                string novoItem = "Item " + (inventarioIndex + 1);
                inventario[inventarioIndex] = novoItem;
                inventarioIndex++;
                Debug.Log("Item adicionado: " + novoItem);
            }
            else
            {
                Debug.Log("Inventário cheio.");
            }
        }

        if (Input.GetKeyDown(KeyCode.U))  // Pressione 'U' para usar um item (por exemplo)
        {
            if (inventarioIndex > 0)
            {
                string itemUsado = inventario[0];
                Debug.Log("Usando item: " + itemUsado);

                // Remover item do inventário
                for (int i = 0; i < inventario.Length - 1; i++)
                {
                    inventario[i] = inventario[i + 1];
                }
                inventario[inventario.Length - 1] = null;
                inventarioIndex--;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
