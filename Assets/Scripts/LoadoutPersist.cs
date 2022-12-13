using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutPersist : MonoBehaviour {
    public static LoadoutPersist Instance;
    
    private static readonly Dictionary<string, PlayerAbilities.AllAbilities> AbilitiesMap = new Dictionary<string, PlayerAbilities.AllAbilities>()
    {
        {"Flamethrower", PlayerAbilities.AllAbilities.Flamethrower},
        {"Rapid Fire", PlayerAbilities.AllAbilities.RapidFire},
        {"Electrorang", PlayerAbilities.AllAbilities.Electrorang},
        {"Piercing Laser", PlayerAbilities.AllAbilities.PiercingLaser},
        {"Plasma Burst", PlayerAbilities.AllAbilities.PlasmaBurst},
        {"Charge Field", PlayerAbilities.AllAbilities.ChargeField}
    };

    private PlayerAbilities.AllAbilities left;
    private PlayerAbilities.AllAbilities right;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StoreAbilities(string leftChoice, string rightChoice) {
        left = AbilitiesMap[leftChoice];
        right = AbilitiesMap[rightChoice];
    }

    public PlayerAbilities.AllAbilities GetLeft() {
        return left;
    }

    public PlayerAbilities.AllAbilities GetRight() {
        return right;
    }
}
