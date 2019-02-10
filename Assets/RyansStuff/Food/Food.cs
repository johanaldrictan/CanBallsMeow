using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Food : MonoBehaviour
{ 
    public float foodValue = 0.1f;

    void OnCollisionEnter2D(Collision2D other)
    {
        FoodCollector foodCollector = other.collider.GetComponent<FoodCollector>();
        if (foodCollector != null)
        {
            foodCollector.HandleFood(foodValue);
            Object.Destroy(this.gameObject);
        }

        // Stick to a floor. Dunno if people want this, but why not.
        // if (other.collider.GetComponent<FoodScript>() == null)
        // {
        //     my_rigidbody.bodyType = RigidbodyType2D.Kinematic;
        // }
    }
}
