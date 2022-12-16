using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Figure : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float movementSpeed;

    [SerializeField] int travelDirection;
    [SerializeField] int strength;
    [SerializeField] float attackFrequency;

    public bool enemy;

    [SerializeField] Image healthFill;
    [SerializeField] SpriteRenderer minimapIcon;

    [SerializeField] int health;
    int maxHealth;

    List<Figure> targetingThisFigure = new List<Figure>();
    Figure target;
    Barracks targetBarracks;
    Figure blockingFigure;
    Figure blockedByFigure;

    LevelManager levelManager;

    float timeSinceAttack = 0;

    bool dead = false;

    bool setupDone = false;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        timeSinceAttack = attackFrequency;
        maxHealth = health;
    }

    public void SetupCreature(bool flipX, bool isEnemy, LevelManager lm) {
        levelManager = lm;
        sprite.flipX = flipX;
        enemy = isEnemy;
        setupDone = true;
        if (flipX) {
            travelDirection = 1;
            minimapIcon.flipX = true;
        } else {
            travelDirection = -1;
            minimapIcon.flipX = false;
            healthFill.transform.parent.transform.localScale = new Vector3(healthFill.transform.parent.transform.localScale.x*-1, healthFill.transform.parent.transform.localScale.y, healthFill.transform.parent.transform.localScale.z);
        }
        if (enemy) {
            minimapIcon.color = Color.red;
        } else {
            minimapIcon.color = Color.green;
        }
    }

    void Update() {
        healthFill.fillAmount = (float)health / maxHealth;

        if (dead || !setupDone) return;
        if(target != null || targetBarracks != null) {
            timeSinceAttack += Time.deltaTime;
            CheckAttack();
        } else if (blockedByFigure == null) {
            timeSinceAttack = attackFrequency;
            animator.SetBool("Walking", true);
            transform.position = new Vector3(transform.position.x + (travelDirection * movementSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        } else {
            timeSinceAttack = attackFrequency;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Figure figure = collision.gameObject.GetComponent<Figure>();
        Barracks barracks = collision.gameObject.GetComponent<Barracks>();
        if (figure != null) {
            animator.SetBool("Walking", false);
            if(figure.enemy != enemy) {
                figure.targetingThisFigure.Add(this);
                target = figure;
            } else {
                switch (travelDirection) {
                    case 1:
                        if (figure.transform.position.x < transform.position.x) return;
                        break;
                    case -1:
                        if (figure.transform.position.x > transform.position.x) return;
                        break;
                }
                blockedByFigure = figure;
                figure.blockingFigure = this;
            }
        } else if (barracks != null) {
            animator.SetBool("Walking", false);
            if (barracks.enemy != enemy) {
                barracks.targetingThisBarracks.Add(this);
                targetBarracks = barracks;
            } else {
                //madeItAcross = true;
                //Debug.Log("THIS SHOULD PROBABLY NOT BE HAPPENING");
            }
        }
    }

    private void CheckAttack() {
        if(timeSinceAttack >= attackFrequency) {
            animator.SetTrigger("Attack");
            if(target != null) {
                target.TakeDamage(strength);
            } else if (targetBarracks != null) {
                targetBarracks.TakeDamage(strength, this);
            }
            timeSinceAttack = 0;    
        }
    }

    public async void TakeDamage(int damage) {
        if (health <= 0) return;
        await Task.Delay(250);
        //animator.SetTrigger("Hit");
        sprite.color = Color.red;
        health -= damage;
        if(health <= 0) {
            Die();
        } else {
            await Task.Delay(200);
            sprite.color = Color.white;
        }
    }

    private async void Die() {
        dead = true;
        Destroy(GetComponent<CapsuleCollider2D>());
        if (blockingFigure != null) {
            blockingFigure.UnblockFigure();
        }
        foreach(Figure f in targetingThisFigure) {
            f.StopTargeting();
        }
        await Task.Delay(50);
        sprite.color = Color.white;
        animator.SetTrigger("Death");
        await Task.Delay(700);
        LeanTween.value(gameObject, 1f, 0f, .5f).setOnUpdate((float val) => {
            Color c = sprite.color;
            c.a = val;
            sprite.color = c;
        });
        await Task.Delay(500);
        Destroy(gameObject);
    }

    public async void UnblockFigure() {
        await Task.Delay(500);
        blockedByFigure = null;
        if(blockingFigure != null) {
            blockingFigure.UnblockFigure();
            blockingFigure = null;
        }
    }

    public void StopTargeting() {
        target = null;
        targetBarracks = null;
        if (blockingFigure != null) {
            blockingFigure.UnblockFigure();
            blockingFigure = null;
        }
    }

    private async void HealUp(int amount) {
        if (dead || health >= maxHealth) return;
        sprite.color = Color.green;
        health += amount;
        await Task.Delay(50);
        sprite.color = Color.white;
    }

    private void OnMouseDown() {
        if (enemy) {
            TakeDamage(1 + levelManager.upgrades);
        } else {
            HealUp(1 + levelManager.upgrades);
        }
    }
}
