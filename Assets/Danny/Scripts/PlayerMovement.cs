using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float dashSideForce;
    [SerializeField] float dashUpForce;
    [SerializeField] float timeForDash;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;

    CircleCollider2D m_CircleCollider;
    Rigidbody2D m_RigidBody;

    float dashTimer = 0f;
    bool canDash = false;
    KeyCode keyPressed;

    bool m_IsGrounded = true;
    // Start is called before the first frame update
    void Start()
    {
        m_CircleCollider = GetComponent<CircleCollider2D>();
        m_RigidBody = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        CheckIfGrounded();
        Dashing();
        Move();
    }

    void Move()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        if (!m_IsGrounded)
        {
            m_RigidBody.velocity = new Vector3(Mathf.Abs(m_RigidBody.velocity.x) * axis, m_RigidBody.velocity.y);
        }
        else
        {
            m_RigidBody.velocity = new Vector3(speed * axis, m_RigidBody.velocity.y);
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

    void Dash()
    {
        Debug.Log(m_IsGrounded);
        if (dashTimer > timeForDash)
        {
            ResetDash();
        }
        else
        {
            if (Input.GetKeyDown(keyPressed))
            {
                if (keyPressed == left)
                    m_RigidBody.AddForce(new Vector2(-dashSideForce, dashUpForce));
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

        Vector3 pos = transform.position + Vector3.down * m_CircleCollider.bounds.extents.y;
        if(Physics2D.Raycast(pos, Vector3.down, 0.01f).collider != null)
        {
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
            ResetDash();
        }
    }
}
