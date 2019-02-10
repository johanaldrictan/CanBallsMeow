using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FoodScript : MonoBehaviour
{
    private Sprite[] foodArt;

    public float foodValue = 0.1f;

    void Start()
    {
        // Get random food art
        if (foodArt == null)
        {
            foodArt = Resources.LoadAll<Sprite>("FoodArt");
        }

        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = foodArt[Random.Range(0, foodArt.Length)];
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        FoodCollectorScript foodCollector = other.collider.GetComponent<FoodCollectorScript>();
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
