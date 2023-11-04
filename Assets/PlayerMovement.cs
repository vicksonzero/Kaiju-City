using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;
    public float airDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [SerializeField]
    private bool readyToJump = true;

    [Header("Key binds")]
    public KeyCode jumpKey;

    [Header("Ground Check")]
    public LayerMask whatIsGround;

    [SerializeField]
    private bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;


    private Vector3 moveDirection;

    private Rigidbody rb;

    public Vector3 velo;

    // Start is called before the first frame update
    void Start()
    {
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }

    private void FixedUpdate()
    {
        
        grounded = Physics.Raycast(
            transform.position, Vector3.down, 0.2f, whatIsGround);
        MovePlayer();
        velo = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {

        MyInput();
        SpeedControl();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.2f);
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.drag = grounded
            ? groundDrag
            : airDrag;

        rb.AddForce(
            moveDirection.normalized * (moveSpeed * (grounded ? 1 : airMultiplier) * 10f),
            ForceMode.Force);
    }

    void SpeedControl()
    {
        var horizontalVelo = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (horizontalVelo.magnitude > moveSpeed)
        {
            var limitedVelo = horizontalVelo.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelo.x, rb.velocity.y, limitedVelo.z);
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    }
}