using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    bool isGrounded;
    Vector3 velocity, desiredVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GroundCheck();
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        float speedChange = acceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, speedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, speedChange);

        rb.velocity = velocity;
        Debug.Log(velocity);

        isGrounded = false;
    }

    void Move()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(xMove, 0, zMove).normalized;
        desiredVelocity = direction * movementSpeed;

        //velocity.y = rb.velocity.y;
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
            velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
    }

    void GroundCheck()
    {
        if (Physics.CheckSphere(transform.position, 0.1f, groundMask))
        {
            isGrounded = true;
        }
    }
}
