using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    #region OLD
    //public bool[] levelsDone = new bool[5] { false, false, false, false, false };

    //public bool alreadyBegunLevel = false;
    //public bool hasMetTommy = false;

    //public bool secondConversation = false;
    //public bool thirdConversation = false;
    //public bool fourthConversation = false;
    //public bool fifthConversation = false;
    //public bool sixthConversation = false;

    //public bool pushedTheButton = false;

    //public bool gotSecretPickup = false;

    //public int ranDialogueIndex = 0;
    //public int teleportDialogueIndex = 0;
    //public int apDialogueIndex = 0;
    //public int redirectDialogueIndex = 0;
    //public int deathDialogueIndex = 0;
    //public int penguinDialogueIndex = 0;
    #endregion

    public bool firstTimeAtMenu = true;

    public bool endofShow = true;
    public bool leftCutscene = false;
    public bool gameCompleted = false;

    public static ProgressManager instance;
    //public bool[] levelsStared = new bool[3] {false,false,false};
    public int currentLevel = -1;
    public float firstPlanetDestoryed = 0f;
    public float secondPlanetDestoryed = 0f;
    

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    //public void ResetLevels() {
    //    levelsStared = new bool[3] { false, false, false };
    //}
}
