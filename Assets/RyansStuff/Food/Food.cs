using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
// Apparently theres two foods? im lost
public class Food : MonoBehaviour
{ 
    public float foodValue = 0.1f;

    // private static float gravity = 0.5f; // Unity Units per second^2
    // private float velY = 0; // Unity Units per second
    // private Rigidbody2D m_Rigidbody2D;

    // void Start()
    // {
    //     this.m_Rigidbody2D = this.GetComponent<Rigidbody2D>();
    // }

    // void FixedUpdate()
    // {
    //     velY -= gravity * Time.fixedDeltaTime;
    //     this.m_Rigidbody2D.MovePosition(this.m_Rigidbody2D.position + Vector2.up * velY * Time.fixedDeltaTime);
    // }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("COLLIDE");

        // TODO: Ask next meeting to make a new layer. Im not sure if itll blow everyone's stuff up so yeah.
        if (other.collider.tag == "Food")
        {
            // This is a terrible solution bc extra memory.
            // Also they still bounce off each other for a frame.
            Physics2D.IgnoreCollision(other.collider, other.otherCollider, true);
            return;
        }

        FoodCollector foodCollector = other.collider.GetComponent<FoodCollector>();
        if (foodCollector != null)
        {
            foodCollector.HandleFood(this.foodValue);
            Object.Destroy(this.gameObject);
        }

        // Stick to a floor. Dunno if people want this, but why not.
        // if (other.collider.GetComponent<FoodScript>() == null)
        // {
        //     my_rigidbody.bodyType = RigidbodyType2D.Kinematic;
        // }
    }
}
