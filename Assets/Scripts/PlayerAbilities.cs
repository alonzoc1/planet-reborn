using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public enum AllAbilities {
        None,
        EnergyBurst
    }

    public AllAbilities primaryAbility;
    public AllAbilities secondaryAbility;
    public List<AllAbilities> passiveAbilities;

    private GameObject primaryAbilityObj;
    private GameObject secondaryAbilityObj;
    private Abilities abilities;

    private void Start() {
        abilities = gameObject.GetComponentInChildren<Abilities>();
        primaryAbilityObj = abilities.GetAbilityGameObject(primaryAbility);
        secondaryAbilityObj = abilities.GetAbilityGameObject(secondaryAbility);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            ActivateAbility(primaryAbility, primaryAbilityObj);
        else if (Input.GetKeyDown(KeyCode.Mouse1))
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
        if (!target.activeInHierarchy)
            target.SetActive(true);
        yield return new WaitForSeconds(time);
        target.SetActive(false);
    }
    
    private void EnergyBurst(GameObject abilityObject) {
        StartCoroutine(EnableForTime(2.0f, abilityObject));
    }
}
