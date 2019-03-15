using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    //For Dashing
    public KeyCode left;
    public KeyCode right;
    public float dashSideForce = 300f;
    public float dashUpForce = 100f;
    public float timeForDash = 0.5f;
    float dashTimer = 0f;
    bool canDash = false;
    bool dashing = false;
    KeyCode keyPressed;
    public float maxDashSpeed = 12f;
    //End


    public const float jumpSquat = 5; // frames. Melee Yoshi's.

    //For Death, Stocks, and Respawning
    private Vector3 respawnPosition = new Vector3(-0.11f, 10, 0);
    private bool blockDeathEvent = true; //To make sure DieAndRespawn() doesn't continually get called. Will later move from Update() to only fire on event
    public int stocks = 3; // 9 if we wanna be techinically correct
    SpriteRenderer sr; //To hide the game object when they are KOd and to render them again when they respawn.
    public CinemachineVirtualCamera vcam;
    public CinemachineTargetGroup vgroup;

    public bool isAlive = true;

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
    private CatAudio m_CatAudio;

    [SerializeField]
    private PlayerState current_state; // {get => current_state; set => current_state = ChangeState(value);}
    private Collider2D lastGround = null;
    private float maxSpeed = 5;
    private int airActions = 2;
    private int jumpSquatTimer = 0;

    private bool jumpQueued;
    private bool fallQueued;

    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        m_foodCollector = GetComponent<FoodCollector>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_CatAudio = GetComponent<CatAudio>();
        sr = GetComponentInChildren<SpriteRenderer>();

        current_state = PlayerState.FALL;

        // TODO: In merge, replace variable in prefab, instead of doing this junk
        m_RigidBody.gravityScale = 2;

        //Check Cinemachine
        if (GameObject.Find("CM vcam1") != null)
        {
            vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        }

        if (GameObject.Find("TargetGroup1") != null)
        {
            vgroup = GameObject.Find("TargetGroup1").GetComponent<CinemachineTargetGroup>();
        }

        //StartCoroutine(DoDeath());
    }

    void Update()
    {
        // Drops inputs if in fixed update >:(
        if (Input.GetKeyDown(jumpKey)) {jumpQueued = true;}
        if (Input.GetKeyDown(fallKey)) {fallQueued = true;}
        Dashing();

    }

    void FixedUpdate()
    {
        animator.SetBool("jump", false);
        animator.SetBool("dash", false);
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
            m_RigidBody.AddForce(Vector2.right * Math.Sign(axis) * 3, ForceMode2D.Impulse);
            if (current_state == PlayerState.GROUND)
            {
                float scaleFactor = transform.localScale.y;
                transform.localScale = new Vector3(-Math.Sign(axis) * scaleFactor, scaleFactor, 1);
            }
        }
        else
        {
            Vector2 old_vel = m_RigidBody.velocity;
            m_RigidBody.velocity = Vector2.left * Math.Sign(m_RigidBody.velocity.x) * 0.2f + m_RigidBody.velocity;
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
        if(dashing)
            clamped.x = Math.Min(this.maxDashSpeed, Math.Max(-this.maxDashSpeed, clamped.x));
        else
            clamped.x = Math.Min(this.maxSpeed, Math.Max(-this.maxSpeed, clamped.x));
        clamped.y = Math.Max(-15, clamped.y); // TODO: Fix magic number
        animator.SetFloat("velX", clamped.x / maxSpeed);
        animator.SetFloat("velY", clamped.y);
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
            m_CatAudio.PlayCatMeow();
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
            m_CatAudio.PlayCatMeow();
            return;
        }
    }

    IEnumerator FallThrough(Collider2D lastGround)
    {
        Physics2D.IgnoreCollision(lastGround, this.m_BoxCollider2D, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(lastGround, this.m_BoxCollider2D, false);
    }

    void Dashing()
    {
        if (current_state != PlayerState.GROUND)
            canDash = false;
        if(canDash)
        {
            Dash();
            dashTimer += Time.deltaTime;
        }
        StartDash();
    }

    void StartDash()
    {
        if(Input.GetKeyDown(left))
        {
            canDash = true;
            keyPressed = left;
        }
        else if(Input.GetKeyDown(right))
        {
            canDash = true;
            keyPressed = right;
        }
    }
    void ResetDash()
    {
        canDash = false;
        dashTimer = 0;
    }

    void Dash()
    {
        if(dashTimer > timeForDash)
        {
            ResetDash();
        }
        else
        {
            if(Input.GetKeyDown(keyPressed))
            {
                current_state = PlayerState.DASH;
                dashing = true;
            }
        }
    }

    private void DoDash()
    {
        //Debug.Log("Dashing");
        if (keyPressed == left)
            m_RigidBody.AddForce(new Vector2(-dashSideForce, dashUpForce), ForceMode2D.Impulse);
        else
            m_RigidBody.AddForce(new Vector2(dashSideForce, dashUpForce), ForceMode2D.Impulse);
        animator.SetBool("dash", true);
        ResetDash();
        current_state = PlayerState.FALL;
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
            //Debug.Log(airActions);
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
        m_CatAudio.PlayCatAttack();
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
            animator.SetBool("jump", true);
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
            //Debug.Log("Landed" + Time.frameCount);
            lastGround = other.collider;
            airActions = 2;
            current_state = PlayerState.GROUND;
            dashing = false;
        }
    }

    void DoDeath()
    {
        print("DYING");
        blockDeathEvent = true;
        stocks -= 1;
        if (stocks <= 0)
        {
            isAlive = false;
            GameObject.Find("CatDeathAudio").GetComponent<AudioSource>().Play();
            //Destroy(gameObject);
        }
        else
        {
            m_CatAudio.PlayCatDeath();
            transform.localScale = new Vector3(0.2986914f, 0.2986914f, 0.2986914f);
            sr.enabled = false;
            vcam.LookAt = null;
            //yield return new WaitForSeconds(0.2f);
            Invoke("Respawn", .2f);
        }
    }

    void Respawn()
    {
        transform.position = respawnPosition;
        transform.rotation = Quaternion.identity;
        sr.enabled = true;
        vcam.LookAt = vgroup.transform;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Death"))
        {
            Invoke("DoDeath", 0);
        }
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
