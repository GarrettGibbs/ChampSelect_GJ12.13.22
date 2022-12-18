using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] Barracks barracks;
    int count = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        count++;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        count--;
    }

    private void Update() {
        barracks.canSpawn = count == 0;
    }
}
