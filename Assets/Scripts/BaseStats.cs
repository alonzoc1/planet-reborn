using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <c>BaseStats</c> Provides base class for all character GameObjects and contains commonly used stats
/// </summary>
public class BaseStats : MonoBehaviour {
    // State enum
    public enum State {
        Normal,
        Stunned,
        Dead
    }

    public enum Buffs {
        SpeedBuff
    }

    public Dictionary<Buffs, Vector3> activeBuffs = new Dictionary<Buffs, Vector3>();

    public float movementSpeed = 5.0f;
    public int maxHealth = 100;
    public State state = State.Normal;

    [SerializeField]
    protected int currentHealth;

    public int GetCurrentHealth() {
        return currentHealth;
    }

    private void Update() {
        if (activeBuffs.Count > 0) {
            // Run down cooldowns on any active buffs
            Dictionary<Buffs, Vector3> newBuffs = new Dictionary<Buffs, Vector3>();
            foreach (KeyValuePair<Buffs, Vector3> kvp in activeBuffs) {
                Vector3 buff = activeBuffs[kvp.Key];
                buff.x -= Time.deltaTime;
                if (buff.x >= 0f)
                    newBuffs[kvp.Key] = buff;
            }

            activeBuffs = newBuffs;
        }
    }

    public float GetMovementSpeed() {
        if (activeBuffs.ContainsKey(Buffs.SpeedBuff))
            return movementSpeed * activeBuffs[Buffs.SpeedBuff].y;
        return movementSpeed;
    }

    /// <summary>
    /// Use this to heal or damage a character. If character is reduced to 0 or lower, sets state to "dead"
    /// </summary>
    /// <param name="modifyBy">Negative if damage, positive if healing</param>
    /// <returns>Returns state</returns>
    public virtual State ChangeCurrentHealth(int modifyBy) {
        currentHealth += modifyBy;
        if (currentHealth <= 0) { // dead
             currentHealth = 0;
             state = State.Dead;
        } else if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        return state;
    }

    /// <summary>
    /// Use this to change the max health of a character midgame (such as retrieving a buff)
    /// </summary>
    /// <param name="modifyBy">Negative if damage, positive if healing</param>
    public virtual void ChangeMaxHealth(int modifyBy)
    {
        maxHealth += modifyBy;
        if (maxHealth <= 0)
        { // Prevents setting health to unwanted values, will reset max/current HP to 1
            maxHealth = 1;
            currentHealth = 1;
        }
        // Below should not be reached atm but covers case if debuffs are added
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

    }
}
