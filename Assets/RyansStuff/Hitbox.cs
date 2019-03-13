using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float lifetime = 0.16f;
    void Start()
    {
        Destroy(this, lifetime);
    }
}
