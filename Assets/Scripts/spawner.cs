using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnRate = 3;
    public static spawner spawn;

    private float nextSpawn = 0f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn){
            nextSpawn = Time.time + spawnRate;

            Instantiate(enemy, transform.position, enemy.transform.rotation); 
        }
    }

    public void SetSpawnRate(int newRate) {
        if(spawnRate >= 0){
            spawnRate += newRate;
        } 
    }
}
