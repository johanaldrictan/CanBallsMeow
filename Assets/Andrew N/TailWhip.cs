using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailWhip : MonoBehaviour
{
    public KeyCode whipAttack;
    public GameObject tail;
    private GameObject tailClone;

    public float attackDuration = .16f;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private Vector3 tempPosition;
    private Vector3 tempScale;

    private BoxCollider2D m_BoxCollider;
    private FoodCollector m_FoodCollector;

    // Start is called before the first frame update
    void Start()
    {
        m_BoxCollider = this.GetComponent<BoxCollider2D>();
        m_FoodCollector = this.GetComponent<FoodCollector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer < 0 && isAttacking == true) // Put cat back in normal position
        {
            flip();
            isAttacking = false;
            Destroy(tailClone);
        }
        else if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        }


        if (Input.GetKeyDown(whipAttack) && isAttacking == false) // Whip attack
        {
            attackTimer = attackDuration;
            isAttacking = true;
            flip();
            tempPosition = transform.position;
            if (transform.localScale.x < 0)
            {
                tempPosition.x += transform.localScale.y * 4;
            }
            else {
                tempPosition.x -= transform.localScale.y * 4;
            }
            tempPosition.y += transform.localScale.y * 1;
            tailClone = GameObject.Instantiate(tail, tempPosition, Quaternion.identity);
            tailClone.GetComponent<Hitbox>().creatorFatness = m_FoodCollector.catFatness * -150;
            // if (transform.localScale.x > 0)
            // {
            tempScale = transform.localScale;
            // tempScale.x *= -1;
            tailClone.transform.localScale = tempScale * 5;
            // }
            Physics2D.IgnoreCollision(m_BoxCollider, tailClone.GetComponent<CircleCollider2D>());
        }

    }

    private void flip()
    {
        Vector3 theScale = transform.Find("Animation").localScale;
        theScale.x *= -1;
        transform.Find("Animation").localScale = theScale; // flip
    }


}
