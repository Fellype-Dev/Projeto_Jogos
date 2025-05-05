using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("Movimentação")]
    public float speed = 5f;
    public float gravity = -45.0f;
    public float jumpHeight = 2f;
    private float yVelocity;
    private bool isGrounded;
    CharacterController cc;
    public Animator animator;

    [Header("Interação com HD")]
    public float interactionRadius = 2f;
    public bool hasHD = false;
    public bool hdInserted = false;
    public bool transferComplete = false;
    public TransferManager transferManager;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movimento
        isGrounded = cc.isGrounded;

        if (isGrounded && yVelocity < 0)
            yVelocity = -2f;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        cc.Move(move * speed * Time.deltaTime);

        animator.SetBool("IsWalking", move.magnitude > 0.1f);
        animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        yVelocity += gravity * Time.deltaTime;
        Vector3 velocity = new Vector3(0, yVelocity, 0);
        cc.Move(velocity * Time.deltaTime);

        // Interação com objetos (HD, computador, servidor)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);
            foreach (Collider hit in hits)
            {
                GameObject obj = hit.gameObject;

                // Pegar HD
                if (!hasHD && obj.CompareTag("HD"))
                {
                    hasHD = true;
                    obj.SetActive(false);
                    Debug.Log("HD coletado.");
                    return;
                }

                // Inserir no computador
                if (hasHD && obj.CompareTag("Computador") && !hdInserted)
                {
                    hdInserted = true;
                    hasHD = false;
                    Debug.Log("HD inserido no computador.");
                    return;
                }

                // Retirar HD com dados
                if (hdInserted && transferComplete && obj.CompareTag("Computador"))
                {
                    hdInserted = false;
                    hasHD = true;
                    Debug.Log("HD retirado com dados.");
                    return;
                }

                // Inserir no servidor
                if (hasHD && obj.CompareTag("Servidor"))
                {
                    hasHD = false;
                    Debug.Log("Tarefa concluída com sucesso!");
                    return;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
