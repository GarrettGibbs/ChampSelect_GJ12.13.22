using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Figure : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float movementSpeed;

    [SerializeField] int travelDirection;
    [SerializeField] int strength;
    [SerializeField] float attackFrequency;

    public bool enemy;

    [SerializeField] int health;

    List<Figure> targetingThisFigure = new List<Figure>();
    Figure target;
    Barracks targetBarracks;
    Figure blockingFigure;
    Figure blockedByFigure;

    float timeSinceAttack = 0;

    bool dead = false;
    bool madeItAcross = false;

    bool setupDone = false;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        timeSinceAttack = attackFrequency;
    }

    public void SetupCreature(bool flipX, bool isEnemy) {
        sprite.flipX = flipX;
        enemy = isEnemy;
        setupDone = true;
        if (flipX) {
            travelDirection = 1;
        } else {
            travelDirection = -1;
        }
    }

    void Update() {
        if (dead || !setupDone) return;
        if(target != null || targetBarracks != null) {
            timeSinceAttack += Time.deltaTime;
            CheckAttack();
        } else if (blockedByFigure == null && !madeItAcross) {
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
                Debug.Log("THIS SHOULD PROBABLY NOT BE HAPPENING");
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
        await Task.Delay(200);
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
}
