using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -45.0f;
    public float jumpHeight = 2f;
    private float yVelocity;
    private bool isGrounded;
    CharacterController cc;

    public Animator animator; 

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = cc.isGrounded;

        if (isGrounded && yVelocity < 0)
        {
            yVelocity = -2f; 
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        cc.Move(move * speed * Time.deltaTime);

        if (move.magnitude > 0.1f) 
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump"); 
        }

        if (!isGrounded)
        {
            animator.SetBool("IsGrounded", false); 
        }
        else
        {
            animator.SetBool("IsGrounded", true);
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 velocity = new Vector3(0, yVelocity, 0);

        cc.Move(velocity * Time.deltaTime);
    }
}