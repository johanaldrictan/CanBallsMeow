using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Andrew Park

public class PlayerController : MonoBehaviour
{
    public string playerName;
    public float dashSideAcceleration;
    public float dashUpAcceleration;
    public float timeForDash;
    public KeyCode left;
    public KeyCode right;




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


    float dashTimer = 0f;
    bool canDash = false;
    KeyCode keyPressed;

    private void Start()
    {
        extraJumps = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (!isGrounded)
        {
            ResetDash();
        }

        if (isGrounded == true)
        {
            extraJumps = extraJumpValue;
            //Debug.Log(true);
        }


        if (Input.GetKeyDown(jumpKey) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
            //Debug.Log(extraJumps);
            isGrounded = false;
        }


        if (Input.GetKey(rotateLeft) && isGrounded == false)
        {
            RotateLeft();
        }
        if (Input.GetKey(rotateRight) && isGrounded == false)
        {
            RotateRight();
        }
        Dashing();
        Move();


        /*  if (moveInput > 0)
          {
              transform.localScale = new Vector3(-scaleFactor, scaleFactor, scaleFactor);
              //spriteRenderer.flipX = true;
          }
          else if (moveInput < 0)
          {
              transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
              //spriteRenderer.flipX = false;
          }
          */

    }


    private void FixedUpdate()
    {
        
        /*moveInput = Input.GetAxis("Horizontal");
        //Debug.Log(moveInput);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);*/
    }


    void RotateLeft()
    {
        transform.Rotate(Vector3.forward * -90 * Time.deltaTime);
    }
    void RotateRight()
    {
        transform.Rotate(Vector3.forward * 90 * Time.deltaTime);
    }


    void Move()
    {
        float axis = Input.GetAxisRaw(playerName + "Horizontal");

        FlipDirection(axis);
        if (!isGrounded)
        {
            rb.velocity = new Vector3((rb.velocity.x == 0 ? speed * axis : Mathf.Abs(rb.velocity.x) * axis), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector3(speed * axis, rb.velocity.y);
        }
    }

    void FlipDirection(float rawAxis)
    {
        if(rawAxis < 0)
        {
            transform.rotation = Quaternion.identity;
        }
        if(rawAxis > 0)
        {
            transform.rotation = new Quaternion(0, -180, 0, 0);
        }
    }

    void Dashing()
    {
        if (canDash)
        {
            Dash();
            dashTimer += Time.deltaTime;
        } 
        StartDash();
        
    }


    void StartDash()
    {

        if (Input.GetKeyDown(left))
        {
            canDash = true;
            keyPressed = left;
        }
        else if (Input.GetKeyDown(right))
        {
            canDash = true;
            keyPressed = right;
        }
    }

    void Dash()
    {
        //Debug.Log(m_IsGrounded);
        if (dashTimer > timeForDash)
        {
            ResetDash();
        }
        else
        {
            if (Input.GetKeyDown(keyPressed))
            {
                if (keyPressed == left)
                   rb.AddForce(new Vector2(-dashSideAcceleration, dashUpAcceleration), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(dashSideAcceleration, dashUpAcceleration), ForceMode2D.Impulse);
                ResetDash();
            }
        }
    }
    

    void ResetDash()
    {
        canDash = false;
        dashTimer = 0;
    }

}
