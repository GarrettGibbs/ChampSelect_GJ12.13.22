using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarracks : MonoBehaviour
{
    [SerializeField] Barracks barracks;
    [SerializeField] LevelManager levelManager;
    float timeElapsed = 0;
    float timeSinceSpawn = 0;

    float timeBetweenSpawns = 12;

    int[] levelTimes = new int[] {60, 300, 800, 1000, 90000};
    int level = 1;

    int[][] creatureIndexes = new int[][] { new int[] { 0, 1, 2 }, new int[] { 1, 2, 3}, new int[] { 2, 3, 4 }, new int[] { 3, 4, 5 }, new int[] { 4, 5 } };

    private void Start() {
        for (int i = 0; i < levelTimes.Length; i++) {
            levelTimes[i] += Random.Range(-10 - (2 * i), 10 + (2 * i));
        }
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        timeSinceSpawn += Time.deltaTime;

        if(timeElapsed > levelTimes[level - 1]) {
            LevelUp();
        }

        if (timeSinceSpawn >= timeBetweenSpawns) {
            barracks.desiredSpawnIndex = creatureIndexes[level - 1][Random.Range(0, creatureIndexes[level - 1].Length)];
            barracks.totalDesiredSpawns++;
            timeSinceSpawn = 0;
            timeBetweenSpawns = Random.Range(13-level, 22-level);
        }
    }

    private void LevelUp() {
        if (level == 5) return;
        level++;
        barracks.LevelUp();
    }
}
