using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    // prefabs to instantiate
    public GameObject[] foodTypes;

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
            whatToSpawn = Random.Range(0, foodTypes.Length); // random value for what to spawn

            //Debug.Log(whatToSpawn); // show on console which object is spawning

            //instantiate a prefab depending on random value
            Instantiate(foodTypes[whatToSpawn], transform.position, Quaternion.identity);

            //Next spawn time
            nextSpawn = Time.time + spawnRate;

        }
	}
}
