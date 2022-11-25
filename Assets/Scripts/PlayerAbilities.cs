using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerAbilities : MonoBehaviour
{
    public enum AllAbilities { // This stores all abilities that a player can possibly have
        None,
        Flamethrower,
        PiercingLaser
    }

    public AllAbilities primaryAbility; // Ability to use on left click
    public AllAbilities secondaryAbility; // Ability to use on right click
    public List<AllAbilities> passiveAbilities; // Abilities that are having some passive effect

    private AbilityCooldowns cooldowns;
    private AbilityTools primaryAbilityTools; // GameObject containing any effects/colliders/etc of the primary ability
    private AbilityTools secondaryAbilityTools; // Same as above but for secondary ability
    private Abilities abilities; // Reference to Abilities script

    private const string IconBasePath = "Skill_Icons/";

    private void Start() {
        // Note: We use the Abilities script to get the GameObject references since it is both:
        // - Much faster than using GameObject.Find, and
        // - GameObject.Find cannot even 'find' disabled GameObjects
        // Note 2: This Start() must run before AbilityCooldowns Start()!
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
            case AllAbilities.PiercingLaser:
                PiercingLaser(abilityTools);
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

    private void PiercingLaser(AbilityTools abilityTools) {
        StartCoroutine(EnableForTime(5.0f, abilityTools.gameObject, abilityTools));
        // Spawn the trail (spawn as its own object, not as a child)
        Transform abilityToolsTransform = abilityTools.transform;
        GameObject trail = Instantiate(abilityTools.abilityPrefab, abilityToolsTransform.position, abilityToolsTransform.rotation);
        Vector3 destination = abilityTools.GetAim();
        // Larger trail resolution means it takes more frames to be drawn, but it looks a lot better across longer distances
        int trailResolution = (int)(Vector3.Distance(trail.transform.position, destination) / 20) + 1;
        StartCoroutine(HitscanTrailMove(trail, destination, trailResolution));
        // Do damage if the hitscan is a hit
        GameObject aimedTarget = abilityTools.GetAimedTarget();
        if (!ReferenceEquals(aimedTarget, null) && aimedTarget.CompareTag("Enemy")) // Faster than != null I guess :^)
            aimedTarget.GetComponent<EnemyAI>().TakeDamage(abilityTools.damage);
    }

    /**
     * Move trail to a point incredibly quickly for hitscan effects
     */
    private IEnumerator HitscanTrailMove(GameObject trail, Vector3 destination, int framesToWait) {
        float distancePerFrame = Vector3.Distance(trail.transform.position, destination) / framesToWait;
        while (framesToWait > 0) {
            framesToWait--;
            yield return new WaitForEndOfFrame();
            // If we're arriving this frame, snap to destination, otherwise move distancePerFrame
            trail.transform.position = framesToWait > 0 ?
                Vector3.MoveTowards(trail.transform.position, destination, distancePerFrame) : destination;
        }
    }
    
    /**
     * Loads Sprite object. Depending on image size this can take some time when called dynamically, use this sparingly
     */
    public Sprite GetIcon(AbilityCooldowns.AbilitySlots slot) {
        return slot switch {
            AbilityCooldowns.AbilitySlots.LeftSlot => Resources.Load<Sprite>(IconBasePath + primaryAbilityTools.iconName),
            AbilityCooldowns.AbilitySlots.RightSlot => Resources.Load<Sprite>(IconBasePath + secondaryAbilityTools.iconName),
            _ => null
        };
    }
}
