using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lol : MonoBehaviour
{
    //dash variables
    [SerializeField] string playerName;
    [SerializeField] float speed;
    [SerializeField] float dashSideForce;
    [SerializeField] float dashUpForce;
    [SerializeField] float timeForDash;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    public int startingJump = 7; //Default fatness that would get a good jump
    private int doubleJumpCounter = 0;


    private FoodCollector m_foodCollector;
    //private Rigidbody2D m_RigidBody;

    CapsuleCollider2D m_CapsuleCollider;
    Rigidbody2D m_RigidBody;

    float dashTimer = 0f;
    bool canDash = false;
    KeyCode keyPressed;

    bool m_IsGrounded = true;
    // Start is called before the first frame update
    void Start()
    {
        m_foodCollector = GetComponent<FoodCollector>();
        m_CapsuleCollider = GetComponent<CapsuleCollider2D>();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (Input.GetKeyDown("w"))
        {
            playerJump();
        }
    }

    void Movement()
    {
        CheckIfGrounded();
        Dashing();
        Move();
    }

    void Move()
    {
        float axis = Input.GetAxisRaw(playerName + "Horizontal");
        FlipDirection(axis);
        if (!m_IsGrounded)
        {
            if (m_RigidBody.velocity.x == 0)
            {
                m_RigidBody.velocity = new Vector3(speed * axis, m_RigidBody.velocity.y);

            }
            else
            {
                m_RigidBody.velocity = new Vector3(Mathf.Abs(m_RigidBody.velocity.x) * axis, m_RigidBody.velocity.y);
            }
        }
        else
        {
            m_RigidBody.velocity = new Vector3(speed * axis, m_RigidBody.velocity.y);
        }
    }

    void FlipDirection(float rawAxis)
    {
        if (rawAxis < 0)
        {
            transform.rotation = Quaternion.identity;
        }
        if (rawAxis > 0)
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
                {
                    m_RigidBody.AddForce(new Vector2(-dashSideForce, dashUpForce));
                }
                else
                    m_RigidBody.AddForce(new Vector2(dashSideForce, dashUpForce));
                ResetDash();
            }
        }
    }

    void ResetDash()
    {
        canDash = false;
        dashTimer = 0;
    }

    void CheckIfGrounded()
    {

        Vector3 pos = transform.position + Vector3.down * m_CapsuleCollider.bounds.extents.y;
        Debug.DrawLine(pos, pos + Vector3.down * 0.1f);
        //Debug.Log(Physics2D.Raycast(pos, Vector3.down, 0.01f).transform.gameObject.name);
        if (Physics2D.Raycast(pos, Vector3.down, 0.1f).collider != null)
        {
            m_IsGrounded = true;
            doubleJumpCounter = 0;
        }
        else
        {
            m_IsGrounded = false;
            ResetDash();
        }
    }

    void playerJump()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();

        //First Jump Off Ground
        if (m_IsGrounded)
        {
            m_RigidBody.AddForce(new Vector2(0, startingJump - (.2f * m_foodCollector.catFatness)), ForceMode2D.Impulse);
            doubleJumpCounter += 1;
            //print(doubleJumpCounter);
        }

        //Double Jump
        else if ((!m_IsGrounded) && doubleJumpCounter < 2)
        {
            Vector3 velocity = m_RigidBody.velocity;
            velocity.y = 0;
            m_RigidBody.velocity = velocity;
            m_RigidBody.AddForce(new Vector2(0, startingJump - (.2f * m_foodCollector.catFatness)), ForceMode2D.Impulse);
            doubleJumpCounter += 1;
        }
    }
}
