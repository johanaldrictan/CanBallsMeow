using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FoodCollector : MonoBehaviour
{
    public float catFatness = 1; // 100%
    // Rigidbody2D my_rigidbody;
    private CatAudio m_CatAudio;

    void Start()
    {
        // my_rigidbody = this.GetComponent<Rigidbody2D>();
        m_CatAudio = GetComponent<CatAudio>();
    }

    public void HandleFood(float diff)
    {
        catFatness += diff;
        transform.localScale *= catFatness / (catFatness - diff);
        m_CatAudio.PlayCatEat();
        // OnSizeChange(catFatness, diff);
    }

    // public void OnSizeChange(float currentFatness, float deltaFatness)
    // {
    //     print(transform.localScale);
    //     // GameObject.Find("Fatness").GetComponent<UnityEngine.UI.Text>().text = (currentFatness * 100) + "%";
    // }
}
