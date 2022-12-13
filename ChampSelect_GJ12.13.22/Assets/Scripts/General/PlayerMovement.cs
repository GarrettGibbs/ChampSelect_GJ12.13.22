using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public LevelManager levelManager;
    [SerializeField] CharacterController2D controller;
    [SerializeField] Animator animator;
    [SerializeField] float runSpeed = 40f;

    [SerializeField] int hitPoints = 3;
    [SerializeField] bool isDead = false;
    bool restartingLevel = false;

    [SerializeField] GameObject[] hearts;
    [SerializeField] GameObject actionIndicator;

    float horizantalMove = 0f;
    bool jump = false;
    bool stopJump = false;
    bool crouch = false;

    float checkJumpTime = .1f;
    float timeSinceJump = 0f;

    float hitGrace = 1f;
    float timeSinceHit = 0f;

    void Update() {
        if (levelManager.gameEnd) return;
        if (isDead) {
            if (restartingLevel) return;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Death") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))) {
                restartingLevel = true;
                OnDeath();
            }
            return;
        }

        timeSinceJump -= Time.deltaTime;
        timeSinceHit += Time.deltaTime;
        horizantalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizantalMove));

        if (Input.GetButtonDown("Jump")) {
            timeSinceJump = checkJumpTime;
        } else if (Input.GetButtonUp("Jump")) {
            stopJump = true;
        }
        if(timeSinceJump > 0) {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        //if (Input.GetButtonDown("Crouch")) {
        //    crouch = true;
        //} else if (Input.GetButtonUp("Crouch")) {
        //    crouch = false;
        //}
        crouch = false;
    }

    private async void OnDeath() {
        Time.timeScale = 1f;
        levelManager.circleTransition.CloseBlackScreen();
        await Task.Delay(1000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnLanding() {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouch(bool isCrouching) {
        animator.SetBool("IsCrouching", isCrouching);
    }

    private void FixedUpdate() {
        //move character
        controller.Move(horizantalMove * Time.fixedDeltaTime, crouch, jump, stopJump);
        jump = false;
        stopJump = false;
    }

    public void OnHit() {
        if (isDead) return;
        if(timeSinceHit > hitGrace) {
            levelManager.audioManager.PlaySound("Hit");
            hitPoints--;
            timeSinceHit = 0f;
            if (hitPoints <= 0) {
                levelManager.respawning = true;
                isDead = true;
                animator.SetBool("Dead", true);
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                levelManager.audioManager.PlaySound("Death");
            }
            UpdateHealth();
            animator.SetTrigger("Hit");
        }
    }

    public void RestoreHealth() {
        if (hitPoints >= 3) return;
        hitPoints++;
        UpdateHealth();
    }

    public void UpdateHealth() {
        for (int i = 0; i < hearts.Length; i++) {
            if(hitPoints > i) {
                hearts[i].SetActive(true);
            } else {
                hearts[i].SetActive(false);
            }
        }
    }

    //public void ActivateSpeedBoost() {
    //    print("WOOOOOOO!");
    //}
}
