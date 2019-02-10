using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateSpawner : MonoBehaviour {

    public float horizontalTranslateRange;          // var for length of horizontal translation

    private bool translateRighBool;                 // var to determine whether to go right or left

    private Vector3 originalPosition;

    // Use this for initialization
    void Start () {
        originalPosition = transform.position;      //Original position of generator
        translateRighBool = true;                   //Initially translate to the right
    }
	
	// Update is called once per frame
	void Update () {
        //Hover upwards if less than a certain distance from original
        if ((transform.position.x < (originalPosition.x + horizontalTranslateRange)) && translateRighBool)
        {
            transform.Translate(Vector2.right * Time.deltaTime);
        }
        //Hover downwards if less than a certain distance from original
        else
        {
            translateRighBool = false;
            transform.Translate(Vector2.left * Time.deltaTime);
            if (transform.position.x < (originalPosition.x - horizontalTranslateRange))
            {
                translateRighBool = true;
            }
        }
    }
}
