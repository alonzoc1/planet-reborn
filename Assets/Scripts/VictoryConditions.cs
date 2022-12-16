using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class VictoryConditions : MonoBehaviour
{
    public enum VictoryModes { // All possible victory modes
        None,
        DefeatAll,
        DefeatNumber,
        TouchVictoryObject,
        DefendTeleporter
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
    public float surviveTime;
    public Teleporter teleporter;

    private int enemyCount = 0;
    private ObjectiveTrackerUI objectives;
    private float timeLeft;
    private bool teleporterActive;
    private bool playerNearTeleporter;
    private float angerEnemyCounter;

    private void OnEnable() {
        objectives = GameObject.FindGameObjectWithTag("ObjectiveTracker").GetComponent<ObjectiveTrackerUI>();
        switch (thisScene) {
            case SceneLibrary.Level1:
                objectives.SetMessage("exit");
                break;
            case SceneLibrary.Shop:
                objectives.SetMessage("shop");
                break;
            case SceneLibrary.Level1Revamp:
                objectives.SetMessage("activate");
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
            case VictoryModes.DefendTeleporter:
                timeLeft = surviveTime;
                teleporterActive = false;
                playerNearTeleporter = false;
                angerEnemyCounter = 8f;
                break;
            case VictoryModes.None:
                Debug.LogWarning("Victory Mode not set... See GameManager object and set VictoryConditions");
                break;
        }

    }

    private void OnDisable() {
        if (victoryMode is VictoryModes.DefeatAll or VictoryModes.DefeatNumber)
            EventManager.OnEnemyDeath -= EnemyDeath;
    }

    private void Update() {
        if (teleporterActive && playerNearTeleporter) {
            timeLeft -= Time.deltaTime;
            angerEnemyCounter -= Time.deltaTime;
            objectives.SetMessage("defendTeleporter", (int)timeLeft + 1);
            teleporter.SetGlowProgress(1f - (timeLeft / surviveTime));
            if (timeLeft <= 0)
                Victory();
            if (angerEnemyCounter <= 0 && playerNearTeleporter) {
                angerEnemyCounter = 8f;
                AngerEnemies(1f - (timeLeft/surviveTime));
            }
        }
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

    public void TeleporterActivated() {
        teleporterActive = true;
    }

    public void SetPlayerInTeleporterRange(bool inRange) {
        playerNearTeleporter = inRange;
    }

    private void AngerEnemies(float chance) {
        // Percent change each enemy starts following the player
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            if (Random.Range(0f, 1f) < chance)
                enemy.GetComponent<EnemyAI>().Anger();
        }
    }
}
