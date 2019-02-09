using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class FoodScript : MonoBehaviour
{
    private Sprite[] foodArt;
    Rigidbody2D my_rigidbody;

    public float foodValue = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        my_rigidbody = this.GetComponent<Rigidbody2D>();

        // Get random food art
        if (foodArt == null)
        {
            foodArt = Resources.LoadAll<Sprite>("FoodArt");
        }

        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = foodArt[Random.Range(0, foodArt.Length)];
        
        // Fix size
        // Transform transform = this.GetComponent<Transform>();
        // float scaleBy = spriteRenderer.sprite.pixelsPerUnit / spriteRenderer.sprite.texture.width;
        // scaleBy /= transform.localScale.x;
        // transform.localScale *= scaleBy;

        // CircleCollider2D collider = this.GetComponent<CircleCollider2D>();
        // collider.radius /= scaleBy;
    }

    // Update is called once per frame
    void Update()
    {

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
        if (other.collider.GetComponent<FoodScript>() == null)
        {
            my_rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
