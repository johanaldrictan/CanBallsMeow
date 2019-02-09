using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyanTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("EmptyCat").GetComponent<FoodCollectorScript>().onSizeChange += tempCatChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void tempCatChange(float newSize, float diff)
    {
        print("Testing Delegate!");
        GameObject.Find("EmptyCat").transform.localScale *= newSize / (newSize - diff);
    }
}
