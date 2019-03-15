using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleVictory : MonoBehaviour
{
    public GameObject victoryScreen1;
    public GameObject victoryScreen2;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Orange Tabby Cat").GetComponent<PlayerController2>().isAlive == false) {
            Time.timeScale = 0f;
            victoryScreen2.SetActive(true);
        }
        else if (GameObject.Find("Gray Variant").GetComponent<PlayerController2>().isAlive == false) {
            Time.timeScale = 0f;
            victoryScreen1.SetActive(true);
        }
    }
}
