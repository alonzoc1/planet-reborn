using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public enum AllAbilities { // This stores all abilities that a player can possibly have
        None,
        Flamethrower,
        SniperShot
    }

    public AllAbilities primaryAbility; // Ability to use on left click
    public AllAbilities secondaryAbility; // Ability to use on right click
    public List<AllAbilities> passiveAbilities; // Abilities that are having some passive effect

    private AbilityCooldowns cooldowns;
    private AbilityTools primaryAbilityTools; // GameObject containing any effects/colliders/etc of the primary ability
    private AbilityTools secondaryAbilityTools; // Same as above but for secondary ability
    private Abilities abilities; // Reference to Abilities script

    private void Start() {
        // Note: We use the Abilities script to get the GameObject references since it is both:
        // - Much faster than using GameObject.Find, and
        // - GameObject.Find cannot even 'find' disabled GameObjects
        abilities = gameObject.GetComponentInChildren<Abilities>();
        primaryAbilityTools = abilities.GetAbilityGameObject(primaryAbility).GetComponent<AbilityTools>();
        secondaryAbilityTools = abilities.GetAbilityGameObject(secondaryAbility).GetComponent<AbilityTools>();

        cooldowns = GameObject.FindGameObjectWithTag("CooldownsUI").GetComponent<AbilityCooldowns>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldowns.GetAbilityReady(AbilityCooldowns.AbilitySlots.LeftSlot)) {
            // Activate left click ability
            cooldowns.StartCooldown(AbilityCooldowns.AbilitySlots.LeftSlot, primaryAbilityTools.cooldown);
            ActivateAbility(primaryAbility, primaryAbilityTools);
        } else if (Input.GetKeyDown(KeyCode.Mouse1) && cooldowns.GetAbilityReady(AbilityCooldowns.AbilitySlots.RightSlot)) {
            // Activate right click ability
            cooldowns.StartCooldown(AbilityCooldowns.AbilitySlots.RightSlot, secondaryAbilityTools.cooldown);
            ActivateAbility(secondaryAbility, secondaryAbilityTools);
        }
    }
    
    private void ActivateAbility(AllAbilities ability, AbilityTools abilityTools) {
        switch (ability) {
            case AllAbilities.Flamethrower:
                Flamethrower(abilityTools);
                break;
            default:
                Debug.Log("Ability not set/found");
                break;
        }
    }

    IEnumerator EnableForTime(float time, GameObject target, AbilityTools tools) {
        // Activate object for some set time
        if (!target.activeInHierarchy)
        {
            target.SetActive(true);
            tools.IncrementActivationId();
            yield return new WaitForSeconds(time);
        }
        target.SetActive(false);
    }
    
    private void Flamethrower(AbilityTools abilityTools) {
        StartCoroutine(EnableForTime(2.0f, abilityTools.gameObject, abilityTools));
    }

    public string GetIconName(AbilityCooldowns.AbilitySlots slot) {
        return slot switch {
            AbilityCooldowns.AbilitySlots.LeftSlot => primaryAbilityTools.iconName,
            AbilityCooldowns.AbilitySlots.RightSlot => secondaryAbilityTools.iconName,
            _ => null
        };
    }
}
