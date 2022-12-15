using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPurchasing : MonoBehaviour
{
    [SerializeField] Barracks barracks;

    [SerializeField] Button golemButton;
    [SerializeField] SpawnableCreature golemSpawn;
    float timeSinceGolemSpawn = 0f;

    private void SpawnCreature(int index) {

    }
}
