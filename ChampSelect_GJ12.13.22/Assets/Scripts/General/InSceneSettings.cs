using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InSceneSettings : MonoBehaviour
{
    [SerializeField] GameObject settingPanel;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Toggle fullscreenToggle;
    bool SettingsOpen = false;

    readonly string fullscreenKey = "fullscreen";

    private void Start() {
        if(PlayerPrefs.GetInt(fullscreenKey, 1) == 1) {
            fullscreenToggle.isOn = true;
        } else {
            fullscreenToggle.isOn = false;
        }
        ToggleFullscreen();
    }

    public void ToggleSettingsPanel() {
        //levelManager.audioManager.PlaySound("Click");
        //if (levelManager.respawning || levelManager.dialogueManager.inConversation || levelManager.gameEnd) return;
        if (!SettingsOpen) {
            levelManager.inSettings = true;
            SettingsOpen = true;
            settingPanel.SetActive(true);
            Time.timeScale = 0;
        } else {
            levelManager.inSettings = false;
            SettingsOpen = false;
            settingPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleSettingsPanel();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            RestartScene();
        }
    }

    public async void RestartScene() {
        //levelManager.audioManager.PlaySound("Click");
        //if (levelManager.respawning || levelManager.gameEnd) return;
        levelManager.respawning = true;
        Time.timeScale = 1f;
        levelManager.circleTransition.CloseBlackScreen();
        levelManager.progressManager.firstTimeAtMenu = false;
        await Task.Delay(1000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public async void ReturnToMenu() {
        //levelManager.audioManager.PlaySound("Click");
        //if (levelManager.respawning || levelManager.gameEnd) return;
        levelManager.respawning = true;
        Time.timeScale = 1f;
        levelManager.circleTransition.CloseBlackScreen();
        await Task.Delay(1000);
        SceneManager.LoadScene(0);
    } 

    public void ToggleFullscreen() {
        //levelManager.audioManager.PlaySound("Click");
        if (fullscreenToggle.isOn) {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt(fullscreenKey, 1);
        } else {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt(fullscreenKey, 0);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
