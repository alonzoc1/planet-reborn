using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void EnemyDeath();

    public delegate void PlayerDeath();
    public delegate void VictoryObjectTouched();

    public static event PlayerDeath OnPlayerDeath;
    public static event EnemyDeath OnEnemyDeath;
    public static event VictoryObjectTouched OnVictoryObjectTouched;

    public static void ReportEnemyDeath() {
        // Why use events instead of just making each enemy
        // have a reference to VictoryConditions?
        // Because each enemy takes more memory to store that reference,
        // so its better to use events in this case
        OnEnemyDeath?.Invoke();
    }

    public static void ReportPlayerDeath() {
        OnPlayerDeath?.Invoke();
    }

    public static void ReportVictoryObjectTouched() {
        OnVictoryObjectTouched?.Invoke();
    }
}
