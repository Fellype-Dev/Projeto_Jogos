using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -45.0f;
    public float jumpHeight = 2f;
    private float yVelocity;
    private bool isGrounded;
    public Transform cameraTransform;
    CharacterController cc;

    public Animator animator; 

    void Start()
    {
        cc = GetComponent<CharacterController>();
        
        // Garante que o cursor fique travado
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleCameraRotation();
        UpdateAnimations();
    }

    void HandleMovement()
    {
        isGrounded = cc.isGrounded;

        if (isGrounded && yVelocity < 0)
        {
            yVelocity = -2f; 
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Movimento relativo à direção em que o jogador está olhando
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        cc.Move(move * speed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }

        yVelocity += gravity * Time.deltaTime;
        cc.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    void HandleCameraRotation()
    {
        // Mantém a rotação vertical da câmera independente do personagem
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
        
        // Atualiza animações de movimento
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("IsWalking", isMoving);

        // Atualiza animações de pulo
        animator.SetBool("IsGrounded", isGrounded);
    }
}