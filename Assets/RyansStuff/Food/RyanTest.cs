using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyanTest : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("EmptyCat").GetComponent<FoodCollectorScript>().onSizeChange += tempCatChange;
    }

    void tempCatChange(float newSize, float diff)
    {
        print("Testing Delegate!");
        GameObject.Find("EmptyCat").transform.localScale *= newSize / (newSize - diff);
    }
}
