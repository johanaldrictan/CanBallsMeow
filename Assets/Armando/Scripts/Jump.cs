using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    public int startingJump = 7; //Default fatness that would get a good jump
    public int catFatness = 0; //The higher this number, the lower the jump will be
    private int doubleJumpCounter = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            playerJump();
        }
    
    }

    void playerJump()
    {
        rb = GetComponent<Rigidbody2D>();

        //First Jump Off Ground
        if (isGrounded())
        {
            rb.AddForce(new Vector2(0, startingJump - (.2f * catFatness)), ForceMode2D.Impulse);
            doubleJumpCounter += 1;
            print(doubleJumpCounter); 
        }

        //Double Jump
        else if((!isGrounded()) && doubleJumpCounter < 2)
        {
            Vector3 velocity = rb.velocity;
            velocity.y = 0;
            rb.velocity = velocity;
            rb.AddForce(new Vector2(0, startingJump - (.2f * catFatness)), ForceMode2D.Impulse);
            doubleJumpCounter += 1;
        }
    }

    bool isGrounded()
    {
        cc = GetComponent<CircleCollider2D>();
        Vector3 pos = transform.position + Vector3.down * cc.bounds.extents.y;
        if(Physics2D.Raycast(pos, Vector3.down, 0.01f).collider != null)
        {
            doubleJumpCounter = 0;
            return true;
        }
        return false;
    }
}
