using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FoodCollector : MonoBehaviour
{
    public float catFatness = 1; // 100%
    // Rigidbody2D my_rigidbody;
    private CatAudio m_CatAudio;
    private Transform headTransform;

    void Start()
    {
        // my_rigidbody = this.GetComponent<Rigidbody2D>();
        m_CatAudio = GetComponent<CatAudio>();
        headTransform = transform.Find("Animation/bone_1/bone_2/bone_3/bone_4");
    }

    public void HandleFood(float diff)
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

        print(catFatness);
        // OnSizeChange(catFatness, diff);
    }

    // public void OnSizeChange(float currentFatness, float deltaFatness)
    // {
    //     print(transform.localScale);
    //     // GameObject.Find("Fatness").GetComponent<UnityEngine.UI.Text>().text = (currentFatness * 100) + "%";
    // }
}
