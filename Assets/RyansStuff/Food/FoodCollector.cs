using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(Rigidbody2D))]
public class FoodCollector : MonoBehaviour
{
    public float catFatness = 1; // 100%
    // Rigidbody2D my_rigidbody;
    private CatAudio m_CatAudio;
    private Transform headTransform;
    private Rigidbody2D m_RigidBody;

    public TextMeshProUGUI fatness; 

    void Start()
    {
        // my_rigidbody = this.GetComponent<Rigidbody2D>();
        m_CatAudio = GetComponent<CatAudio>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        headTransform = transform.Find("Animation/bone_1/bone_2/bone_3/bone_4");
    }

    public void HandleFood(float diff, float knockback)
    {
        if (catFatness + diff < 0.75)
        {
            diff = 0.75f - catFatness;
        }

        catFatness += diff;
        transform.localScale *= catFatness / (catFatness - diff);
        headTransform.localScale *= (catFatness - diff * 0.5f) / catFatness;
        // print(headTransform.lossyScale);
        // It actually keeps relative size well, but an illusion makes it look like its shrinking.

        // Everyone's favorite:
        float magic_number = 0.0625f;

        transform.position = transform.position + Vector3.up * magic_number * catFatness / (catFatness - diff);
        if (diff > 0)
        {
            m_CatAudio.PlayCatEat();
        }
        else
        {
            // take knockback
            print(knockback);
            m_RigidBody.AddForce(Vector2.up * Mathf.Abs(knockback) + Vector2.right * knockback * 10);
        }

        print(catFatness);
        OnSizeChange(catFatness);
    }

    public void OnSizeChange(float currentFatness)
    {
        //print(transform.localScale);
        fatness.text = (currentFatness * 100) + "%";
    }
}
