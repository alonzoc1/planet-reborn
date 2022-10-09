using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public enum AllAbilities { // This stores all abilities that a player can possibly have
        None,
        EnergyBurst
    }

    public AllAbilities primaryAbility; // Ability to use on left click
    public AllAbilities secondaryAbility; // Ability to use on right click
    public List<AllAbilities> passiveAbilities; // Abilities that are having some passive effect

    private GameObject primaryAbilityObj; // GameObject containing any effects/colliders/etc of the primary ability
    private GameObject secondaryAbilityObj; // Same as above but for secondary ability
    private Abilities abilities; // Reference to Abilities script

    private void Start() {
        // Note: We use the Abilities script to get the GameObject references since it is both:
        // - Much faster than using GameObject.Find, and
        // - GameObject.Find cannot even 'find' disabled GameObjects
        abilities = gameObject.GetComponentInChildren<Abilities>();
        primaryAbilityObj = abilities.GetAbilityGameObject(primaryAbility);
        secondaryAbilityObj = abilities.GetAbilityGameObject(secondaryAbility);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Activate left click ability
            ActivateAbility(primaryAbility, primaryAbilityObj);
        else if (Input.GetKeyDown(KeyCode.Mouse1)) // Activate right click ability
            ActivateAbility(secondaryAbility, secondaryAbilityObj);
    }
    
    private void ActivateAbility(AllAbilities ability, GameObject abilityObject) {
        switch (ability) {
            case AllAbilities.EnergyBurst:
                EnergyBurst(abilityObject);
                break;
            default:
                Debug.Log("Ability not set/found");
                break;
        }
    }

    IEnumerator EnableForTime(float time, GameObject target) {
        // Activate object for some set time
        if (!target.activeInHierarchy)
            target.SetActive(true);
        yield return new WaitForSeconds(time);
        target.SetActive(false);
    }
    
    private void EnergyBurst(GameObject abilityObject) {
        StartCoroutine(EnableForTime(2.0f, abilityObject));
    }
}
