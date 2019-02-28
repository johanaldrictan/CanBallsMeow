using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float jumpForce = 8;
    public int extraJumpValue = 6;
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode fallKey = KeyCode.S;
    public String xAxis = "Horizontal";
    public String yAxis = "Vertical";
    public Transform groundCheck;
    public float checkRadius = 0.5f;
    public LayerMask whatIsGround;

    // private float moveInput;
    // private Rigidbody2D rb;
    // private bool isGrounded;
    // private int extraJumps;

    ///////////////////////////////////////////////////////////

    enum PlayerState {GROUND, DASH, FALL, ATTACK}

    // private Rigidbody2D rb;
    // private CircleCollider2D cc;

    private FoodCollector m_foodCollector;
    private BoxCollider2D m_BoxCollider2D;
    private Rigidbody2D m_RigidBody;
    private SpriteRenderer m_SpriteRenderer;

    private PlayerState current_state; // {get => current_state; set => current_state = ChangeState(value);}
    private Collider2D lastGround = null;
    private float maxSpeed = 5;
    private int airActions = 2;

    void Start()
    {
        m_foodCollector = GetComponent<FoodCollector>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        current_state = PlayerState.FALL;
    }

    void FixedUpdate()
    {
        switch (current_state)
        {
            case PlayerState.GROUND : this.DoGround(); break;
            case PlayerState.DASH : this.DoDash(); break;
            case PlayerState.FALL : this.DoFall(); break;
            case PlayerState.ATTACK : this.DoAttack(); break;
        }

        // You can always drift.
        float axis = Input.GetAxisRaw(xAxis);
        if (axis != 0)
        {
            m_RigidBody.AddForce(Vector2.right * axis * 100, ForceMode2D.Impulse);
            float scaleFactor = transform.localScale.y;
            // m_SpriteRenderer.flipX = axis > 0;
            transform.localScale = new Vector3(-Math.Sign(axis) * scaleFactor, scaleFactor, 1);
            // print(transform.localScale);
        }
        else
        {
            Vector2 old_vel = m_RigidBody.velocity;
            m_RigidBody.velocity = Vector2.left * Math.Sign(m_RigidBody.velocity.x) * 0.7f + m_RigidBody.velocity;
            if (old_vel.x * m_RigidBody.velocity.x < 0)
            {
                m_RigidBody.velocity = new Vector2(0, m_RigidBody.velocity.y);
            }
        }
        this.ClampSpeed();
    }

    private void ClampSpeed()
    {  
        // Isn't making new objects every frame bad?
        Vector2 clamped = new Vector2(m_RigidBody.velocity.x, m_RigidBody.velocity.y);
        clamped.x = Math.Min(this.maxSpeed, Math.Max(-this.maxSpeed, clamped.x));
        clamped.y = Math.Max(-30, clamped.y); // TODO: Fix magic number
        m_RigidBody.velocity = clamped;
    }

    // private PlayerState ChangeState(PlayerState value)
    // {
    //     switch (value)
    //     {
    //         case PlayerState.GROUND : this.ChangeGround(); break;
    //         case PlayerState.DASH : this.ChangeDash(); break;
    //         case PlayerState.FALL : this.ChangeFall(); break;
    //         case PlayerState.ATTACK : this.ChangeAttack(); break;
    //     }
    //     return value;
    // }

    private void DoGround()
    {
        maxSpeed = 5;
        // airActions = 2;
        // if (!Physics2D.OverlapCircle(groundCheck.position + m_RigidBody.velocity.y * Vector3.up, checkRadius, whatIsGround))
        if (m_RigidBody.velocity.y < -0.1)
        {
            Debug.Log("nani");
            current_state = PlayerState.FALL;
            return;
        }
        if (Input.GetKeyDown(jumpKey))
        {
            Debug.Log("grounded");
            current_state = PlayerState.FALL;
            m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, jumpForce);
            return;
        }
        if (Input.GetKeyDown(fallKey) && lastGround.name.Contains("OneWay"))
        {
            current_state = PlayerState.FALL;
            StartCoroutine(FallThrough(lastGround));
            lastGround = null;
            // Physics2D.IgnoreCollision(lastGround, this.m_BoxCollider2D, true);
            // m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, jumpForce);
            // Debug.Log("JUMP");
            return;
        }
    }

    IEnumerator FallThrough(Collider2D lastGround)
    {
        Physics2D.IgnoreCollision(lastGround, this.m_BoxCollider2D, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(lastGround, this.m_BoxCollider2D, false);
    }

    private void DoDash()
    {
    }

    private void DoFall()
    {
        // if (m_RigidBody.velocity.y < 0 && Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround))
        // {
        //     // Collider2D new_ground = Physics2D.OverlapCircleAll(groundCheck.position, checkRadius, whatIsGround)[0];
        //     // if (new_ground != last_ground)
        //     // {
        //     current_state = PlayerState.GROUND;
        //     m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, 0);
        //     Debug.Log("Landed" + Time.frameCount);
        //     return;
        //     // }
        // }

        // print("falllling");
        // m_RigidBody.AddForce(Vector2.down * 3);
        if (Input.GetKey(jumpKey) && m_RigidBody.velocity.y > 0)
        {
            m_RigidBody.gravityScale = 1;
        }
        else
        {
            m_RigidBody.gravityScale = 2;
        }

        if (Input.GetKeyDown(jumpKey) && airActions > 0)
        {
            Debug.Log(airActions);
            airActions -= 1;
            m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, extraJumpValue);
        }

        // float axis = Input.GetAxisRaw("Horizontal");
        // m_RigidBody.speed * axis, m_RigidBody.velocity.y);
    }

    private void DoAttack()
    {
    }

    // BUG: If you get too big, you never exit the one way platforms since they're all one collider.
    // However, you end up not getting grounded either, while still standing on them.
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag != "Food" &&
                m_RigidBody.velocity.y <= 0 &&
                other.GetContact(0).normal.y > 0)
        {
            m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, 0);
            Debug.Log("Landed" + Time.frameCount);
            lastGround = other.collider;
            airActions = 2;
            current_state = PlayerState.GROUND;
            print(current_state);
        }
    }

    // void OnCollisionExit2D(Collision2D other)
    // {
    //     if (lastGround == other.collider)
    //     {
    //         Physics2D.IgnoreCollision(lastGround, this.m_BoxCollider2D, false);
    //         lastGround = null;
    //     }        
    // }

    // //dash variables
    // [SerializeField] float speed;
    // [SerializeField] float dashSideForce;
    // [SerializeField] float dashUpForce;
    // [SerializeField] float timeForDash;
    // [SerializeField] KeyCode left;
    // [SerializeField] KeyCode right;
    // public int startingJump = 7; //Default fatness that would get a good jump
    // private int doubleJumpCounter = 0;

    // float dashTimer = 0f;
    // bool canDash = false;
    // KeyCode keyPressed;

    // bool m_IsGrounded = true;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     m_foodCollector = GetComponent<FoodCollector>();
    //     m_CircleCollider = GetComponent<CircleCollider2D>();
    //     m_RigidBody = GetComponent<Rigidbody2D>();   
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     Movement();
    //     if (Input.GetKeyDown("w"))
    //     {
    //         playerJump();
    //     }
    // }

    // void Movement()
    // {
    //     CheckIfGrounded();
    //     Dashing();
    //     ClampVel();
    //     Move();
    // }

    // void Move()
    // {
    //     float axis = Input.GetAxisRaw("Horizontal");
    //     if (!m_IsGrounded)
    //     {
    //         m_RigidBody.velocity = new Vector3(Mathf.Abs(m_RigidBody.velocity.x) * axis, m_RigidBody.velocity.y);
    //     }
    //     else
    //     {
    //         m_RigidBody.velocity = new Vector3(speed * axis, m_RigidBody.velocity.y);
    //     }
    // }

    // void Dashing()
    // {
    //     if (canDash)
    //     {
    //         Dash();
    //         dashTimer += Time.deltaTime;
    //     }
    //     StartDash();
    // }


    // void StartDash()
    // {

    //     if(Input.GetKeyDown(left))
    //     {
    //         canDash = true;
    //         keyPressed = left;
    //     }
    //     else if(Input.GetKeyDown(right))
    //     {
    //         canDash = true;
    //         keyPressed = right;
    //     }
    // }

    // void Dash()
    // {
    //     //Debug.Log(m_IsGrounded);
    //     if (dashTimer > timeForDash)
    //     {
    //         ResetDash();
    //     }
    //     else
    //     {
    //         if (Input.GetKeyDown(keyPressed))
    //         {
    //             if (keyPressed == left)
    //                 m_RigidBody.AddForce(new Vector2(-dashSideForce, dashUpForce));
    //             else
    //                 m_RigidBody.AddForce(new Vector2(dashSideForce, dashUpForce));
    //             ResetDash();
    //         }
    //     }
    // }

    // void ResetDash()
    // {
    //     canDash = false;
    //     dashTimer = 0;
    // }

    // void CheckIfGrounded()
    // {

    //     Vector3 pos = transform.position + Vector3.down * m_CircleCollider.bounds.extents.y;
    //     if(Physics2D.Raycast(pos, Vector3.down, 0.01f).collider != null)
    //     {
    //         m_IsGrounded = true;
    //         doubleJumpCounter = 0;
    //     }
    //     else
    //     {
    //         m_IsGrounded = false;
    //         ResetDash();
    //     }
    // }

    // void playerJump()
    // {
    //     rb = GetComponent<Rigidbody2D>();

    //     //First Jump Off Ground
    //     if (m_IsGrounded)
    //     {
    //         rb.AddForce(new Vector2(0, startingJump - (.2f * m_foodCollector.catFatness)), ForceMode2D.Impulse);
    //         doubleJumpCounter += 1;
    //         //print(doubleJumpCounter);
    //     }

    //     //Double Jump
    //     else if ((!m_IsGrounded) && doubleJumpCounter < 2)
    //     {
    //         Vector3 velocity = rb.velocity;
    //         velocity.y = 0;
    //         rb.velocity = velocity;
    //         rb.AddForce(new Vector2(0, startingJump - (.2f * m_foodCollector.catFatness)), ForceMode2D.Impulse);
    //         doubleJumpCounter += 1;
    //     }
    // }
}
