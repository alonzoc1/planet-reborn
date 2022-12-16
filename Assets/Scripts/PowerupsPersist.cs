using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsPersist : MonoBehaviour
{
    public static PowerupsPersist Instance;
    public float damageBuff;
    public float speedBuff;
    public int healthBuffAdditive;
    public float jumpBuff;
    // each buff is just a multiplier that gets applied to playerstats on it's Start() except healthBuffAdditive, which
    // is additive (not a multiplier)

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Reset();
    }

    public void Reset() {
        damageBuff = 1f;
        speedBuff = 1f;
        jumpBuff = 1f;
        healthBuffAdditive = 0;
    }
}
