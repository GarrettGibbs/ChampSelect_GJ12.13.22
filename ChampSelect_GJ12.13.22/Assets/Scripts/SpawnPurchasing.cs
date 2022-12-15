using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPurchasing : MonoBehaviour
{
    [SerializeField] Barracks barracks;
    [SerializeField] LevelManager levelManager;

    [SerializeField] Button golemButton;
    [SerializeField] Image golemTimerFill;
    float timeSinceGolemSpawn = 0f;

    [SerializeField] Button dwarfButton;
    [SerializeField] Image dwarfTimerFill;
    float timeSinceDwarfSpawn = 0f;

    private void Update() {
        timeSinceGolemSpawn += Time.deltaTime;
        golemTimerFill.fillAmount = timeSinceGolemSpawn / levelManager.creatures[0].resetTime;
        if(timeSinceGolemSpawn < levelManager.creatures[0].resetTime || levelManager.currency < levelManager.creatures[0].cost || barracks.timeSinceSpawn < 8) {
            golemButton.interactable = false;
        } else {
            golemButton.interactable = true;
        }

        timeSinceDwarfSpawn += Time.deltaTime;
        dwarfTimerFill.fillAmount = timeSinceDwarfSpawn / levelManager.creatures[1].resetTime;
        if (timeSinceDwarfSpawn < levelManager.creatures[1].resetTime || levelManager.currency < levelManager.creatures[1].cost || barracks.timeSinceSpawn < 8) {
            dwarfButton.interactable = false;
        } else {
            dwarfButton.interactable = true;
        }
    }

    public void SpawnCreature(int index) {
        //check overall timeSinceSawn
        //if (levelManager.currency < levelManager.creatures[index].cost) return;
        levelManager.currency -= levelManager.creatures[index].cost;
        barracks.SpawnCreature(index);
        switch (index) {
            case 0:
                timeSinceGolemSpawn = 0;
                break;
            case 1:
                timeSinceDwarfSpawn = 0;
                break;
        }
    }
}
