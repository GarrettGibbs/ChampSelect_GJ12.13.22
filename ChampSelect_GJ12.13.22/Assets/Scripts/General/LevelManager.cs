using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject menuCenter;
    public AudioManager audioManager;
    public CircleTransition circleTransition;
    public ProgressManager progressManager;
    public DialogueManager dialogueManager;
    public InSceneSettings inSceneSettings;

    public bool respawning = false;
    public bool gameEnd = false;
    public bool inSettings = false;
    //public bool readyToLeave = false;

    public SpawnableCreature[] creatures;
    public SpawnableCreature[] enemies;

    public float currency = 0;
    [SerializeField] TMP_Text currencyText;

    public int upgrades = 0;

    private int enemyDeaths = 0;

    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;

    [SerializeField] GameObject easyCheck;
    [SerializeField] GameObject normalCheck;
    [SerializeField] GameObject hardCheck;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        circleTransition = FindObjectOfType<CircleTransition>();
        progressManager = FindObjectOfType<ProgressManager>();
        if (player != null) {
            circleTransition.player = player.transform;
        } else if (menuCenter != null) {
            circleTransition.player = menuCenter.transform;
        }
    }

    private async void Start() {
        progressManager.currentLevel = SceneManager.GetActiveScene().buildIndex;
        //audioManager.TransitionMusic(MusicType.Peaceful);
        if (!progressManager.firstTimeAtMenu) {
            await Task.Delay(1000);
            circleTransition.OpenBlackScreen();
        }
        if(easyCheck != null) {
            ChangeDifficuly(progressManager.difficulty);
        } else {
            switch (progressManager.difficulty) {
                case .75f:
                    currency = 100;
                    break;
                case 1f:
                    currency = 25;
                    break;
                case 1.5f:
                    currency = 0;
                    break;
            }
        }
    }

    private void Update() {
        if (progressManager.currentLevel == 0) return;
        currency += (Time.deltaTime + (.5f * upgrades * Time.deltaTime));
        currencyText.text = Mathf.FloorToInt(currency).ToString();
    }

    public async void RestartLevel() {
        await Task.Delay(1000);
        inSceneSettings.RestartScene();
        respawning = true;
    }

    public async void NextLevel() {
        circleTransition.CloseBlackScreen();
        progressManager.firstTimeAtMenu = false;
        await Task.Delay(1000);
        switch (progressManager.currentLevel) {
            case 0:
                SceneManager.LoadScene(1);
                break;
            case 1:
                SceneManager.LoadScene(0);
                break;
        }
    }

    public async void SpecificLevel(int destination) {
        circleTransition.CloseBlackScreen();
        progressManager.firstTimeAtMenu = false;
        await Task.Delay(1000);
        SceneManager.LoadScene(destination);
    }

    public void OnBarracksDeath(bool enemy) {
        if (enemy) {
            enemyDeaths++;
            if(enemyDeaths == 2) {
                OnWin();
            }
        } else {
            OnLose();
        }
    }

    private void OnWin() {
        Time.timeScale = 0;
        audioManager.PlaySound("GameEnd");
        respawning = true;
        WinPanel.SetActive(true);
    }

    private void OnLose() {
        Time.timeScale = 0;
        audioManager.PlaySound("Death");
        respawning = true;
        LosePanel.SetActive(true);
    }

    public void ChangeDifficuly(float input) {
        progressManager.difficulty = input;
        switch (input) {
            case .75f:
                easyCheck.SetActive(true);
                normalCheck.SetActive(false);
                hardCheck.SetActive(false);
                break;
            case 1f:
                easyCheck.SetActive(false);
                normalCheck.SetActive(true);
                hardCheck.SetActive(false);
                break;
            case 1.5f:
                easyCheck.SetActive(false);
                normalCheck.SetActive(false);
                hardCheck.SetActive(true);
                break;
        }
    }
}
