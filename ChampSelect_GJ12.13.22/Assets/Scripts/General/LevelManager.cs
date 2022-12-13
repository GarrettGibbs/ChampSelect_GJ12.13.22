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

    [SerializeField] TMP_Text timerText;

    public bool respawning = false;
    public bool gameEnd = false;
    public bool inSettings = false;
    public bool readyToLeave = false;

    float timeElapsed = 0;

    [SerializeField] Image planet1;
    [SerializeField] TMP_Text planet1Time;
    [SerializeField] Image planet2;
    [SerializeField] TMP_Text planet2Time;
    [SerializeField] Button level2Button;

    private void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        circleTransition = FindObjectOfType<CircleTransition>();
        progressManager = FindObjectOfType<ProgressManager>();
        if(player != null) {
            circleTransition.player = player.transform;
        } else if (menuCenter != null) {
            circleTransition.player = menuCenter.transform;
        }
    }

    private void Start() {
        progressManager.currentLevel = SceneManager.GetActiveScene().buildIndex;
        audioManager.TransitionMusic(MusicType.Peaceful);
        if(!progressManager.firstTimeAtMenu) {
            circleTransition.OpenBlackScreen();
        }
        if(progressManager.currentLevel == 0) {
            CheckWins();
        }
    }

    private void Update() {
        if (timerText != null && !gameEnd) {
            timeElapsed += Time.deltaTime;
            timerText.text = (Mathf.Round(timeElapsed * 100f) / 100f).ToString();
        }
    }

    public async void PlanetDestroyed() {
        gameEnd = true;
        switch (progressManager.currentLevel) {
            case 1:
                if(timeElapsed < progressManager.firstPlanetDestoryed || progressManager.firstPlanetDestoryed == 0) {
                    progressManager.firstPlanetDestoryed = (Mathf.Round(timeElapsed * 100f) / 100f);
                }
                break;
            case 2:
                if (timeElapsed < progressManager.secondPlanetDestoryed|| progressManager.secondPlanetDestoryed == 0) {
                    progressManager.secondPlanetDestoryed = (Mathf.Round(timeElapsed * 100f) / 100f);
                }
                break;
        }
        audioManager.PlaySound("BGM_Victory");
        await Task.Delay(1000);
        NextLevel();
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
                SceneManager.LoadScene(2);
                break;
            case 2:
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

    private void CheckWins() {
        if(progressManager.firstPlanetDestoryed > 0) {
            planet1.color = Color.white;
            planet1Time.text = progressManager.firstPlanetDestoryed.ToString();
            level2Button.interactable = true;
        }
        if (progressManager.secondPlanetDestoryed > 0) {
            planet2.color = Color.white;
            planet2Time.text = progressManager.secondPlanetDestoryed.ToString();
        }
    }
}
