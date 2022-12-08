using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Class <c>AbilityStats</c> Contains commonly used data required for most abilities. Extend as necessary
/// </summary>
public class AbilityTools : MonoBehaviour {
    public PlayerAbilities.AllAbilities abilityName;

    public int
        activationId; // Useful for enemies to track instances of an ability to avoid getting hit twice by same move

    public bool hitsOnlyOnce;
    public bool aimedAbility; // Ability direction needs to be updated each frame (not needed for projectiles)
    public int damage;
    public float cooldown;
    public bool holdButtonAbility; // If true, don't show cooldown in UI since ability is meant to be held
    public bool manualCooldown; // If true, AbilityCooldown won't automatically track the cooldown
    public string iconName; // Filename of icon image in Resources/Skill_Icons folder
    public GameObject abilityPrefab; // Use this if you need to spawn something in for the ability activation
    public float speed; // Used by some abilities that move something

    private Abilities abilities;

    private void Awake() {
        abilities = gameObject.GetComponentInParent<Abilities>();
        activationId = 0;
    }

    public void IncrementActivationId() {
        activationId++;
    }

    public Vector3 GetAim() {
        return abilities.GetAim();
    }

    public GameObject GetAimedTarget() {
        return abilities.GetAimedTarget();
    }

    private void Update() {
        if (aimedAbility)
            gameObject.transform.LookAt(GetAim());
    }
}