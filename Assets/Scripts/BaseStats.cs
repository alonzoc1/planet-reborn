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
    
    public float movementSpeed = 5.0f;
    public int maxHealth = 100;
    public State state = State.Normal;

    [SerializeField]
    protected int currentHealth;

    public int GetCurrentHealth() {
        return currentHealth;
    }

    /// <summary>
    /// Use this to heal or damage a character. If character is reduced to 0 or lower, sets state to "dead"
    /// </summary>
    /// <param name="modifyBy">Negative if damage, positive if healing</param>
    public virtual void ChangeCurrentHealth(int modifyBy) {
        currentHealth += modifyBy;
        if (currentHealth <= 0) { // ded
             currentHealth = 0;
             state = State.Dead;
        } else if (currentHealth > maxHealth)
            currentHealth = maxHealth;
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
