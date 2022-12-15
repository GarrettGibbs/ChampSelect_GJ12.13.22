using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Barracks : MonoBehaviour
{
    [SerializeField] Sprite[] lvl1Barracks = new Sprite[5];
    [SerializeField] Sprite[] lvl2Barracks = new Sprite[5];
    [SerializeField] Sprite[] lvl3Barracks = new Sprite[5];
    [SerializeField] Sprite[] lvl4Barracks = new Sprite[5];
    [SerializeField] Sprite[] lvl5Barracks = new Sprite[5];

    [SerializeField] SpriteRenderer sprite;
    public bool enemy;
    [SerializeField] bool flipX;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] LevelManager levelManager;

    public List<Figure> targetingThisBarracks = new List<Figure>();

    [SerializeField] Image healthFill;

    int health = 100;
    int[] healthAtLevels = new int[5] {100, 200, 500, 1000, 2000};
    int level = 1;

    int SpawnIndex = 0;

    public bool canSpawn = true;
    public float timeSinceSpawn = 8;
    public int totalDesiredSpawns = 0;
    int totalSpawns = 0;

    private void Update() {
        healthFill.fillAmount = (float)health / healthAtLevels[level-1];

        timeSinceSpawn += Time.deltaTime;
        if(totalDesiredSpawns > totalSpawns) {
            SpawnCreature(0);
        }
    }

    public void LevelUp() {
        if (level == 5) return;
        int difference = healthAtLevels[level + 1] - healthAtLevels[level];
        level++;
        health += difference;
        CheckDamageLevel();
    }

    public async void TakeDamage(int damage, Figure f) {
        if (health <= 0) return;
        f.TakeDamage(1000);
        sprite.color = Color.red;
        health -= damage;
        CheckDamageLevel();
        if (health <= 0) {
            Die();
        } else {
            await Task.Delay(200);
            sprite.color = Color.white;
        }
    }

    private void CheckDamageLevel() {
        float healthRatio = (float)health / healthAtLevels[level - 1];
        if(healthRatio > .8f) {
            ResetSprite(0);
        } else if (healthRatio > .6f) {
            ResetSprite(1);
        } else if (healthRatio > .4f) {
            ResetSprite(2);
        } else if (healthRatio > .2f) {
            ResetSprite(3);
        } else {
            ResetSprite(4);
        }
    }

    private void ResetSprite(int damageLevel) {
        switch (level) {
            case 1:
                sprite.sprite = lvl1Barracks[damageLevel];
                break;
            case 2:
                sprite.sprite = lvl2Barracks[damageLevel];
                break;
            case 3:
                sprite.sprite = lvl3Barracks[damageLevel];
                break;
            case 4:
                sprite.sprite = lvl4Barracks[damageLevel];
                break;
            case 5:
                sprite.sprite = lvl5Barracks[damageLevel];
                break;
        }
    }

    private async void Die() {
        //dead = true;
        Destroy(GetComponent<CapsuleCollider2D>());
        foreach (Figure f in targetingThisBarracks) {
            f.StopTargeting();
        }
        await Task.Delay(200);
        sprite.color = Color.white; //TODO ADD explosion animation
        await Task.Delay(700);
        LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) => {
            Color c = sprite.color;
            c.a = val;
            sprite.color = c;
        });
        await Task.Delay(500);
        Destroy(gameObject);
    }

    public async void SpawnCreature(int index = 0) {
        if(!canSpawn || timeSinceSpawn < 8) return;
        totalSpawns++;
        timeSinceSpawn = 0;
        if(enemy) {
            SpawnIndex++;
        } else {
            SpawnIndex--;
        }
        SpawnableCreature creature = levelManager.creatures[index];
        GameObject c = Instantiate(creature.creature, new Vector3(spawnPoint.transform.position.x, creature.yOffset, SpawnIndex), Quaternion.identity);
        await Task.Delay(5);
        c.GetComponent<Figure>().SetupCreature(flipX, enemy);
    }
}
