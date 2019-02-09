﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FoodCollectorScript : MonoBehaviour
{
    public float size = 1; // 100%
    public delegate void OnSizeChange(float newSize, float diff);
    public OnSizeChange onSizeChange;

    Rigidbody2D my_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        my_rigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleFood(float diff)
    {
        size += diff;
        if (onSizeChange != null)
        {
            this.onSizeChange(size, diff);
        }
    }
}
