using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float stealthMovementSpeed;
    [SerializeField] private float turnAcceleration;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundMask;

    bool isGrounded;
    bool desiredJump;
    bool isStealthMode;
    Vector3 velocity, desiredVelocity;
    public Vector3 CurrentVelocity { get { return rb.velocity; } }

    //Refs
    private Rigidbody rb;
    private Animator anim;
    private Transform cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        cam = Camera.main.transform;
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

        Vector3 direction = (cam.right * xMove + cam.forward * zMove).normalized;
        desiredVelocity = isStealthMode ? direction * stealthMovementSpeed : direction * movementSpeed;

        //if (desiredVelocity.magnitude >= 0.1f)
        //{
        //    float targetAngle = Mathf.Atan2(desiredVelocity.x, desiredVelocity.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        //    //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnAcceleration, turnSpeed);

        //    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        //}

        transform.forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up);

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
