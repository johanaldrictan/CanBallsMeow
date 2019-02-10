using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour
{
    private Jump jumpScript;
    // Start is called before the first frame update
    public float weightGain = .3f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //For every collision with the tag "Food", the public catFatness variable from the Jump script will increase by 1 while the scale of the cat is increased by weightGain.
        if (collision.gameObject.tag == "Food")
        {
            jumpScript = GetComponent<Jump>();
            jumpScript.catFatness += 1;

            transform.localScale += new Vector3(weightGain, weightGain, 0);

        }
    }
}
