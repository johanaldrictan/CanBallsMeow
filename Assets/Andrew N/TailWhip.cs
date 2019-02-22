using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailWhip : MonoBehaviour
{

    public KeyCode whipAttack;
    public Vector3 tail;

    public float attackDuration = .03f;
    //private float timeTillNextAttack = .5f;
    private bool isAttacking = false;

    private float attackTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer < 0 && isAttacking == true) // Put cat back in normal position
        {
            flip();
            isAttacking = false;
        }
        else if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        }


        if (Input.GetKeyDown(whipAttack) && isAttacking == false) // Whip attack
        {
            attackTimer = attackDuration;
            isAttacking = true;
            flip();
        }


    }

    private void flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale; // flip
    }

    private void enlarge()
    {

    }

}
