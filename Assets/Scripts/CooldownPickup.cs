using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownPickup : Pickup
{

    public PlayerAbilities playAbilities;
    public GameObject player;

    private AbilityTools primaryAbilityTools; // GameObject containing any effects/colliders/etc of the primary ability
    private AbilityTools secondaryAbilityTools; // Same as above but for secondary ability
    private Abilities abilities; // Reference to Abilities script
    public new void OnTriggerEnter() //Overrides previous OnTriggerEnter
    {
        player = GameObject.Find("Player");
        playAbilities = player.GetComponent<PlayerAbilities>();
        abilities = player.GetComponentInChildren<Abilities>();
        primaryAbilityTools = abilities.GetAbilityGameObject(playAbilities.primaryAbility).GetComponent<AbilityTools>();
        secondaryAbilityTools = abilities.GetAbilityGameObject(playAbilities.secondaryAbility).GetComponent<AbilityTools>();

        primaryAbilityTools.cooldown = primaryAbilityTools.cooldown - 1;
        secondaryAbilityTools.cooldown = secondaryAbilityTools.cooldown - 1;
        PickUp();
    }
}
