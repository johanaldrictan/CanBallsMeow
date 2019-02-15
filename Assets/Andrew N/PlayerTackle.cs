using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTackle : MonoBehaviour
{
    private float timeBtwTackle;
    public float startTimeBtwAttack;

    public float tackleDistance;
    public int damage;


    // Update is called once per frame
    void Update()
    {
        if (timeBtwTackle <= 0)
        {
            if (Input.GetKey(KeyCode.F))
            {
                transform.Translate(Vector2.up * Time.deltaTime *tackleDistance);
            }
            timeBtwTackle = startTimeBtwAttack;
        }
        else
        {
            timeBtwTackle -= Time.deltaTime;
        }
    }
}
