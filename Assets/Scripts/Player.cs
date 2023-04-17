using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float stealthMovementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundMask;

    bool isGrounded;
    bool desiredJump;
    bool isStealthMode;
    Vector3 velocity, desiredVelocity;

    //Refs
    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        GroundCheck();
        Move();
        Jump();

        if (Input.GetButtonDown("Crouch"))
            SwitchStealth();
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        float speedChange = acceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, speedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, speedChange);

        if (desiredJump)
        {
            velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            desiredJump = false;
        }

        rb.velocity = velocity;

        isGrounded = false;
    }

    void Move()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        anim.SetFloat("Strafe", xMove);
        anim.SetFloat("Throttle", zMove);

        Vector3 direction = new Vector3(xMove, 0, zMove).normalized;
        desiredVelocity = isStealthMode ? direction * stealthMovementSpeed : direction * movementSpeed;

        //velocity.y = rb.velocity.y;
    }

    void Jump()
    {
        if (!isGrounded) return;

        desiredJump |= Input.GetButtonDown("Jump");
        if (!desiredJump) return;

        anim.SetBool("IsJumping", true);

        if (isStealthMode)
            SwitchStealth();
    }

    void SwitchStealth()
    {
        isStealthMode = !isStealthMode;
        anim.SetBool("Stealth", isStealthMode);
    }

    void GroundCheck()
    {
        if (Physics.CheckSphere(transform.position, 0.1f, groundMask))
        {
            isGrounded = true;
            anim.SetBool("IsJumping", false);
        }
    }
}
