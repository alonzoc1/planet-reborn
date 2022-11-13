using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryConditions : MonoBehaviour
{
    public enum VictoryModes { // All possible victory modes
        None,
        DefeatAll,
        DefeatNumber,
        //DefeatBoss,
        //SurviveTime,
        //Endless
    }

    public VictoryModes victoryMode = VictoryModes.None; // Change this in editor per scene
    public int defeatNumber = 5;

    private int enemyCount = 0;

    private void OnEnable() {
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

    private int GetEnemyCount() {
        // Call this as few times as possible
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void Victory() {
        Debug.Log("You win :)");
    }
}
