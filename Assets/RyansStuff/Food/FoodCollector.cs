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
        if (catFatness + diff < 0.75)
        {
            diff = 0.75f - catFatness;
        }

        catFatness += diff;
        transform.localScale *= catFatness / (catFatness - diff);
        GameObject.Find("bone_4").transform.localScale *= (catFatness - diff) / catFatness;
        
        // Everyone's favorite:
        float magic_number = 0.0625f;

        transform.position = transform.position + Vector3.up * magic_number * catFatness / (catFatness - diff);
        if (diff > 0)
        {
            m_CatAudio.PlayCatEat();
        }

        print(catFatness);
        // OnSizeChange(catFatness, diff);
    }

    // public void OnSizeChange(float currentFatness, float deltaFatness)
    // {
    //     print(transform.localScale);
    //     // GameObject.Find("Fatness").GetComponent<UnityEngine.UI.Text>().text = (currentFatness * 100) + "%";
    // }
}
