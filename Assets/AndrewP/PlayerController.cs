using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Andrew Park

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public int extraJumpValue;
    public KeyCode jumpKey;
    public KeyCode rotateLeft;
    public KeyCode rotateRight;
    public SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float scaleFactor;


    private float moveInput;
    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJumps;


    private void Start()
    {
        extraJumps = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = extraJumpValue;
            Debug.Log(true);
        }


        if (Input.GetKeyDown(jumpKey) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
            Debug.Log(extraJumps);
            isGrounded = false;
        }
        else if (Input.GetKeyDown(jumpKey) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }


        if (Input.GetKey(rotateLeft) && isGrounded == false)
        {
            RotateLeft();
        }
        if (Input.GetKey(rotateRight) && isGrounded == false)
        {
            RotateRight();
        }


        if (moveInput > 0)
        {
            transform.localScale = new Vector3(-scaleFactor, scaleFactor, scaleFactor);
            //spriteRenderer.flipX = true;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            //spriteRenderer.flipX = false;
        }
    }


    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        moveInput = Input.GetAxis("Horizontal");
        //Debug.Log(moveInput);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }


    void RotateLeft()
    {
        transform.Rotate(Vector3.forward * -90 * Time.deltaTime);
    }
    void RotateRight()
    {
        transform.Rotate(Vector3.forward * 90 * Time.deltaTime);
    }
}
