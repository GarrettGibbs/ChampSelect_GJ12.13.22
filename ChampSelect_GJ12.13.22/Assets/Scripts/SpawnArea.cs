using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] Barracks barracks;

    private void OnTriggerEnter2D(Collider2D collision) {
        barracks.canSpawn = false;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        barracks.canSpawn = true;
    }
}
