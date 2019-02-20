using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FoodCollector : MonoBehaviour
{
    public float catFatness = 1; // 100%
    // Rigidbody2D my_rigidbody;

    void Start()
    {
        // my_rigidbody = this.GetComponent<Rigidbody2D>();
    }

    public void HandleFood(float diff)
    {
        catFatness += diff;
        OnSizeChange(catFatness, diff);
    }

    public void OnSizeChange(float currentFatness, float deltaFatness)
    {
        transform.localScale *= currentFatness / (currentFatness - deltaFatness);
        // GameObject.Find("Fatness").GetComponent<UnityEngine.UI.Text>().text = (currentFatness * 100) + "%";
    }
}
