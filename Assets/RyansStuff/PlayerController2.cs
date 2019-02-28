using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float jumpForce = 12; // Should reach platform
    public float shortJumpForce = 8; // Should reach platform
    public float doubleJumpForce = 10;
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode fallKey = KeyCode.S;
    public String xAxis = "Horizontal";
    public String yAxis = "Vertical";
    // public Transform groundCheck;
    // public float checkRadius = 0.5f;
    public LayerMask whatIsGround;

    public const float jumpSquat = 5; // frames. Melee Yoshi's.

    // private float moveInput;
    // private Rigidbody2D rb;
    // private bool isGrounded;
    // private int extraJumps;

    ///////////////////////////////////////////////////////////

    enum PlayerState {GROUND, DASH, FALL, ATTACK, JUMP_SQUAT}

    // private Rigidbody2D rb;
    // private CircleCollider2D cc;

    private FoodCollector m_foodCollector;
    private BoxCollider2D m_BoxCollider2D;
    private Rigidbody2D m_RigidBody;
    private SpriteRenderer m_SpriteRenderer;

    [SerializeField]
    private PlayerState current_state; // {get => current_state; set => current_state = ChangeState(value);}
    private Collider2D lastGround = null;
    private float maxSpeed = 5;
    private int airActions = 2;
    private int jumpSquatTimer = 0;

    private bool jumpQueued;
    private bool fallQueued;

    void Start()
    {
        m_foodCollector = GetComponent<FoodCollector>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        current_state = PlayerState.FALL;

        // TODO: In merge, replace variable in prefab, instead of doing this junk
        m_RigidBody.gravityScale = 2;
    }

    void Update()
    {
        // Drops inputs if in fixed update >:(
        if (Input.GetKeyDown(jumpKey)) {jumpQueued = true;}
        if (Input.GetKeyDown(fallKey)) {fallQueued = true;}
    }

    void FixedUpdate()
    {
        switch (current_state)
        {
            case PlayerState.GROUND : this.DoGround(); break;
            case PlayerState.DASH : this.DoDash(); break;
            case PlayerState.FALL : this.DoFall(); break;
            case PlayerState.ATTACK : this.DoAttack(); break;
            case PlayerState.JUMP_SQUAT : this.DoJumpSquat(); break;
        }

        // You can always drift.
        float axis = Input.GetAxisRaw(xAxis);
        if (axis != 0)
        {
            m_RigidBody.AddForce(Vector2.right * Math.Sign(axis) * 5, ForceMode2D.Impulse);
            if (current_state == PlayerState.GROUND)
            {
                float scaleFactor = transform.localScale.y;
                transform.localScale = new Vector3(-Math.Sign(axis) * scaleFactor, scaleFactor, 1);
            }
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

        jumpQueued = false;
        fallQueued = false;
        
        // print(current_state);
    }

    private void ClampSpeed()
    {  
        // Isn't making new objects every frame bad?
        Vector2 clamped = new Vector2(m_RigidBody.velocity.x, m_RigidBody.velocity.y);
        clamped.x = Math.Min(this.maxSpeed, Math.Max(-this.maxSpeed, clamped.x));
        clamped.y = Math.Max(-15, clamped.y); // TODO: Fix magic number
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
        maxSpeed = 6;
        // airActions = 2;
        // if (!Physics2D.OverlapCircle(groundCheck.position + m_RigidBody.velocity.y * Vector3.up, checkRadius, whatIsGround))
        if (m_RigidBody.velocity.y < -0.1)
        {
            Debug.Log("nani");
            current_state = PlayerState.FALL;
            return;
        }
        if (jumpQueued)
        {
            jumpQueued = false;
            // Debug.Log("grounded");
            current_state = PlayerState.JUMP_SQUAT;
            jumpSquatTimer = 0;
            return;
        }
        if (fallQueued && lastGround.name.Contains("OneWay"))
        {
            fallQueued = false;
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

    private int unstuck_hack = 0;
    private void DoFall()
    {
        if (m_RigidBody.velocity.y == 0)
        {
            unstuck_hack++;
            if (unstuck_hack > 3) {print("bug hated\nryan exasperated\nfix created\nHACK ACTIVATED"); current_state = PlayerState.GROUND;}
        }
        else {unstuck_hack = 0;}

        if (jumpQueued && airActions > 0)
        {
            jumpQueued = false;
            Debug.Log(airActions);
            airActions -= 1;
            m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, doubleJumpForce);
        }
        if (fallQueued && m_RigidBody.velocity.y <= 0)
        {
            fallQueued = false;
            m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, -15);
        }

        // print("falllling");
        // m_RigidBody.AddForce(Vector2.down * 3);
        // if (Input.GetKey(jumpKey) && m_RigidBody.velocity.y > 0)
        // {
        //     m_RigidBody.gravityScale = 1;
        // }
        // else
        // {
        //     m_RigidBody.gravityScale = 2;
        // }

        // float axis = Input.GetAxisRaw("Horizontal");
        // m_RigidBody.speed * axis, m_RigidBody.velocity.y);
    }

    private void DoAttack()
    {
    }

    private void DoJumpSquat()
    {
        jumpSquatTimer += 1;
        if (jumpSquatTimer > jumpSquat)
        {
            current_state = PlayerState.FALL;
            m_RigidBody.velocity = new Vector2(
                m_RigidBody.velocity.x,
                Input.GetKey(jumpKey) ? jumpForce : shortJumpForce
            );
            return;
        }
    }

    // BUG: If you get too big, you never exit the one way platforms since they're all one collider.
    // However, you end up not getting grounded either, while still standing on them.
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag != "Food" &&
                m_RigidBody.velocity.y <= 0 &&
                other.GetContact(0).normal.y >= 0)
        {
            m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, 0);
            Debug.Log("Landed" + Time.frameCount);
            lastGround = other.collider;
            airActions = 2;
            current_state = PlayerState.GROUND;
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
}
