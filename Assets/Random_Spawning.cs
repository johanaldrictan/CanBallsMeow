using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Spawning : MonoBehaviour {

    // prefabs to instantiate
    public GameObject food1, food2, food3, food4;

    // spawn prefabs once per 2 seconds
    private float spawnRate;

    // variable to set next spawn time
    float nextSpawn = 0f;

    // variable to contain random value
    int whatToSpawn;

    // Update is called once per frame
    void Update () {
        spawnRate = Random.Range(2, 7);
        if (Time.time > nextSpawn) {
            whatToSpawn = Random.Range(1, 5); // random value for what to spawn

            //Debug.Log(whatToSpawn); // show on console which object is spawning

            //instantiate a prefab depending on random value
            switch (whatToSpawn) {
                case 1:
                    Instantiate(food1, transform.position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(food2, transform.position, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(food3, transform.position, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(food4, transform.position, Quaternion.identity);
                    break;
            }

            //Next spawn time
            nextSpawn = Time.time + spawnRate;

        }
	}
}
