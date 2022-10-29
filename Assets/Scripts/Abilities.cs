using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Abilities</c> Stores a direct reference to all ability GameObjects
/// </summary>
public class Abilities : MonoBehaviour {
    public GameObject energyBurst;
    public GameObject sniperShot;
    
    public GameObject GetAbilityGameObject(PlayerAbilities.AllAbilities ability) {
        // This is faster than doing a GameObject.Find, but requires a little more maintenance
        return ability switch {
            PlayerAbilities.AllAbilities.EnergyBurst => energyBurst,
            PlayerAbilities.AllAbilities.SniperShot => sniperShot,
            _ => null
        };
    }
}
