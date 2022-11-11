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
    
    public override void ChangeCurrentHealth(int modifyBy)
    {
        base.ChangeCurrentHealth(modifyBy);
        healthbar.SetValue(currentHealth, maxHealth);
    }
    // Note: If searching for ChangeMaxHealth, found in BaseStats (Override removed due to redundancy)
}
