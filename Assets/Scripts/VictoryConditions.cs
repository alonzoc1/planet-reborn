using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryConditions : MonoBehaviour
{
    public enum VictoryModes { // All possible victory modes
        None,
        DefeatAll,
        DefeatNumber,
        TouchVictoryObject,
        //DefeatBoss,
        //SurviveTime,
        //Endless
    }

    public enum SceneLibrary { // These MUST be in the same order as each scene's index in Build Settings
        MainMenu,
        Level1,
        Shop,
        Level1Revamp,
        Victory
    }

    public VictoryModes victoryMode = VictoryModes.None; // Change this in editor per scene
    public SceneLibrary nextScene;
    public SceneLibrary thisScene;
    public int defeatNumber = 5;

    private int enemyCount = 0;
    private ObjectiveTrackerUI objectives;

    private void OnEnable() {
        objectives = GameObject.FindGameObjectWithTag("ObjectiveTracker").GetComponent<ObjectiveTrackerUI>();
        switch (thisScene) {
            case SceneLibrary.Level1:
                objectives.SetMessage("exit");
                break;
            case SceneLibrary.Shop:
                objectives.SetMessage("shop");
                break;
            default:
                objectives.SetMessage("hide");
                break;
        }
        switch (victoryMode) {
            case VictoryModes.DefeatAll:
                EventManager.OnEnemyDeath += EnemyDeath;
                enemyCount = GetEnemyCount();
                break;
            case VictoryModes.DefeatNumber:
                EventManager.OnEnemyDeath += EnemyDeath;
                enemyCount = GetEnemyCount();
                if (defeatNumber > enemyCount)
                    defeatNumber = enemyCount;
                break;
            case VictoryModes.TouchVictoryObject:
                EventManager.OnVictoryObjectTouched += VictoryObjectTouched;
                break;
            case VictoryModes.None:
                Debug.LogWarning("Victory Mode not set... See GameManager object and set VictoryConditions");
                break;
        }

    }

    private void OnDisable() {
        EventManager.OnEnemyDeath -= EnemyDeath;
    }

    private void EnemyDeath() {
        enemyCount--;
        defeatNumber--;
        switch (victoryMode) {
            case VictoryModes.DefeatAll:
                if (enemyCount <= 0)
                    Victory();
                break;
            case VictoryModes.DefeatNumber:
                if (defeatNumber <= 0)
                    Victory();
                break;
        }
    }

    private void VictoryObjectTouched() {
        Victory();
    }

    private int GetEnemyCount() {
        // Call this as few times as possible
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void Victory() {
        Debug.Log("You win :)");
        SceneManager.LoadScene((int)nextScene);
    }
}
