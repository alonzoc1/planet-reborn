using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>AbilityStats</c> Contains commonly used data required for most abilities. Extend as necessary
/// </summary>
public class AbilityTools : MonoBehaviour {
    public PlayerAbilities.AllAbilities abilityName;
    public int activationId; // Useful for enemies to track instances of an ability to avoid getting hit twice by same move
    public bool hitsOnlyOnce;
    public int damage;
    public float cooldown;
    public string iconName;

    private void Awake() {
        activationId = 0;
    }

    public void IncrementActivationId() {
        activationId++;
    }
}
