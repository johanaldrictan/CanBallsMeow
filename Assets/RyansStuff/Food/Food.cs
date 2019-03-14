using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
// Apparently theres two foods? im lost
public class Food : MonoBehaviour
{ 
    public float foodValue = 0.1f;
    public bool dontDestroy = false;

    // private static float gravity = 0.5f; // Unity Units per second^2
    // private float velY = 0; // Unity Units per second
    private CircleCollider2D m_CircleCollider;

    void Start()
    {
        this.m_CircleCollider = this.GetComponent<CircleCollider2D>();
    }

    // void FixedUpdate()
    // {
    //     velY -= gravity * Time.fixedDeltaTime;
    //     this.m_Rigidbody2D.MovePosition(this.m_Rigidbody2D.position + Vector2.up * velY * Time.fixedDeltaTime);
    // }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("COLLIDE");

        // TODO: Ask next meeting to make a new layer. Im not sure if itll blow everyone's stuff up so yeah.
        // if (other.collider.tag == "Food")
        // {
        //     // This is a terrible solution bc extra memory.
        //     // Also they still bounce off each other for a frame.
        //     Physics2D.IgnoreCollision(other.collider, other.otherCollider, true);
        //     return;
        // }

        FoodCollector foodCollector = other.collider.GetComponent<FoodCollector>();
        if (foodCollector != null)
        {
            Hitbox hitbox = GetComponent<Hitbox>();
            foodCollector.HandleFood(this.foodValue, hitbox == null ? 0 : hitbox.creatorFatness * transform.localScale.x);
            if (!dontDestroy)
            {
                Object.Destroy(this.gameObject);
            }
            else
            {
                Physics2D.IgnoreCollision(this.m_CircleCollider, other.collider);
            }
        }

        // Stick to a floor. Dunno if people want this, but why not.
        // if (other.collider.GetComponent<FoodScript>() == null)
        // {
        //     my_rigidbody.bodyType = RigidbodyType2D.Kinematic;
        // }
    }
}
