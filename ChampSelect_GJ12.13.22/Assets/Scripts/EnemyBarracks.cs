using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarracks : MonoBehaviour
{
    [SerializeField] Barracks barracks;
    [SerializeField] LevelManager levelManager;
    float timeElapsed = 0;
    float timeSinceSpawn = 0;

    float timeBetweenSpawns = 8;

    void Update()
    {
        timeElapsed += Time.deltaTime;
        timeSinceSpawn += Time.deltaTime;
        if(timeSinceSpawn >= timeBetweenSpawns) {
            barracks.totalDesiredSpawns++;
            timeSinceSpawn = 0;
            timeBetweenSpawns = Random.Range(6, 15);
        }
    }
}
