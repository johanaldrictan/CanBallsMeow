using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTackle : MonoBehaviour
{
    /*private Rigidbody2D rb;
    public float tackleSpeed;

    private float tackleTime;
    public float startTackleTime;
    private int direction; 

    void Start() {
        rb = GetComponent<Rigidbody2D>;
        tackleTime = startTackleTime;
    }

    void Update() {
        if (direction == 0) {
            if (Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.Space)) {
                direction = 1;
            } else if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.Space)) {
                direction = 2;
            }
        } else {
            if (tackleTime <= 2) {
                direction = 0;
                tackleTime = startTackleTime;
                rb.velocity = Vector2.zero;
            } else {
                direction -= tackleTime.deltaTime;
                if (direction == 1)
                {
                    rb.velocity = Vector2.right * tackleSpeed;
                }
                else if (direction == 2) {
                    rb.velocity = Vector2.right * tackleSpeed;
                }
            }
        }
    }*/

}
