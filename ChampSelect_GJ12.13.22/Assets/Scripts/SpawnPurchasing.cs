using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPurchasing : MonoBehaviour
{
    [SerializeField] Barracks barracks;
    [SerializeField] LevelManager levelManager;

    [SerializeField] Button golemButton;
    [SerializeField] Image golemTimerFill;
    float timeSinceGolemSpawn = 20f;
    [SerializeField] TMP_Text golemCostText;

    [SerializeField] Button dwarfButton;
    [SerializeField] Image dwarfTimerFill;
    float timeSinceDwarfSpawn = 20f;
    [SerializeField] TMP_Text dwarfCostText;

    [SerializeField] Button fighterButton;
    [SerializeField] Image fighterTimerFill;
    float timeSinceFighterSpawn = 20f;
    [SerializeField] TMP_Text fighterCostText;

    [SerializeField] Button zeusButton;
    [SerializeField] Image zeusTimerFill;
    float timeSinceZeusSpawn = 20f;
    [SerializeField] TMP_Text zeusCostText;

    [SerializeField] Image overallTimerFill;

    private void Update() {
        overallTimerFill.fillAmount =  1 - (barracks.timeSinceSpawn / 4);

        timeSinceGolemSpawn += Time.deltaTime;
        golemTimerFill.fillAmount = timeSinceGolemSpawn / levelManager.creatures[0].resetTime;
        if(timeSinceGolemSpawn < levelManager.creatures[0].resetTime || levelManager.currency < levelManager.creatures[0].cost || barracks.timeSinceSpawn < 4) {
            golemButton.interactable = false;
        } 
        else {
            golemButton.interactable = true;
        }
        if(levelManager.currency < levelManager.creatures[0].cost) {
            golemCostText.color = Color.red;
        } else {
            golemCostText.color = Color.black;
        }

        timeSinceDwarfSpawn += Time.deltaTime;
        dwarfTimerFill.fillAmount = timeSinceDwarfSpawn / levelManager.creatures[1].resetTime;
        if (timeSinceDwarfSpawn < levelManager.creatures[1].resetTime || levelManager.currency < levelManager.creatures[1].cost || barracks.timeSinceSpawn < 4) {
            dwarfButton.interactable = false;
        } else {
            dwarfButton.interactable = true;
        }
        if (levelManager.currency < levelManager.creatures[1].cost) {
            dwarfCostText.color = Color.red;
        } else {
            dwarfCostText.color = Color.black;
        }

        timeSinceFighterSpawn += Time.deltaTime;
        fighterTimerFill.fillAmount = timeSinceFighterSpawn / levelManager.creatures[2].resetTime;
        if (timeSinceFighterSpawn < levelManager.creatures[2].resetTime || levelManager.currency < levelManager.creatures[2].cost || barracks.timeSinceSpawn < 4) {
            fighterButton.interactable = false;
        } else {
            fighterButton.interactable = true;
        }
        if (levelManager.currency < levelManager.creatures[2].cost) {
            fighterCostText.color = Color.red;
        } else {
            fighterCostText.color = Color.black;
        }

        timeSinceZeusSpawn += Time.deltaTime;
        zeusTimerFill.fillAmount = timeSinceZeusSpawn / levelManager.creatures[3].resetTime;
        if (timeSinceZeusSpawn < levelManager.creatures[3].resetTime || levelManager.currency < levelManager.creatures[3].cost || barracks.timeSinceSpawn < 4) {
            zeusButton.interactable = false;
        } else {
            zeusButton.interactable = true;
        }
        if (levelManager.currency < levelManager.creatures[3].cost) {
            zeusCostText.color = Color.red;
        } else {
            zeusCostText.color = Color.black;
        }
    }

    public void SpawnCreature(int index) {
        if (!barracks.canSpawn) return;
        levelManager.currency -= levelManager.creatures[index].cost;
        barracks.SpawnCreature(index);
        switch (index) {
            case 0:
                timeSinceGolemSpawn = 0;
                break;
            case 1:
                timeSinceDwarfSpawn = 0;
                break;
            case 2:
                timeSinceFighterSpawn = 0;
                break;
            case 3:
                timeSinceZeusSpawn = 0;
                break;
        }
    }
}
