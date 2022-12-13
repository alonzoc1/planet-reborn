using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <c>PlayerStats</c> Inherits from BaseStats, used for player character
/// </summary>
public class PlayerStats : BaseStats {
    public float jumpPower = 120f;
    public PlayerHealthBarUI healthbar;

    private void Start() {
        healthbar.SetValue(currentHealth, maxHealth);
    }
    
    public override State ChangeCurrentHealth(int modifyBy)
    {
        // Update current health, and if we died on this instance of damage, report it to the event manager
        if (State.Dead == base.ChangeCurrentHealth(modifyBy))
        {
            state = State.Dead;
            EventManager.ReportPlayerDeath();
        }
            
        healthbar.SetValue(currentHealth, maxHealth);
        return state;
    }
    // Note: If searching for ChangeMaxHealth, found in BaseStats (Override removed due to redundancy)

}
