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
        PiercingLaser,
        RapidFire,
        Electrorang,
        PlasmaBurst,
        ChargeField
    }

    public AllAbilities primaryAbility; // Ability to use on left click
    public AllAbilities secondaryAbility; // Ability to use on right click
    public List<AllAbilities> passiveAbilities; // Abilities that are having some passive effect

    private AbilityCooldowns cooldowns;
    private bool abilityCooldownsLoaded;
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

        abilityCooldownsLoaded = false; // AbilityCooldowns sets this when its ready
    }

    private void Update() {
        if (!abilityCooldownsLoaded || Time.timeScale == 0)
            return;
        if (Input.GetKey(KeyCode.Mouse0) && cooldowns.GetAbilityReady(AbilityCooldowns.AbilitySlots.LeftSlot)) {
            // Activate left click ability
            cooldowns.StartCooldown(AbilityCooldowns.AbilitySlots.LeftSlot, primaryAbilityTools.cooldown, !primaryAbilityTools.holdButtonAbility, primaryAbilityTools.manualCooldown);
            ActivateAbility(primaryAbility, primaryAbilityTools, AbilityCooldowns.AbilitySlots.LeftSlot);
        } else if (Input.GetKey(KeyCode.Mouse1) && cooldowns.GetAbilityReady(AbilityCooldowns.AbilitySlots.RightSlot)) {
            // Activate right click ability
            cooldowns.StartCooldown(AbilityCooldowns.AbilitySlots.RightSlot, secondaryAbilityTools.cooldown, !secondaryAbilityTools.holdButtonAbility, secondaryAbilityTools.manualCooldown);
            ActivateAbility(secondaryAbility, secondaryAbilityTools, AbilityCooldowns.AbilitySlots.RightSlot);
        }
    }
    
    private void ActivateAbility(AllAbilities ability, AbilityTools abilityTools, AbilityCooldowns.AbilitySlots slot) {
        switch (ability) {
            case AllAbilities.Flamethrower:
                Flamethrower(abilityTools);
                break;
            case AllAbilities.PiercingLaser:
                PiercingLaser(abilityTools);
                break;
            case AllAbilities.RapidFire:
                RapidFire(abilityTools);
                break;
            case AllAbilities.Electrorang:
                Electrorang(abilityTools, slot);
                break;
            case AllAbilities.PlasmaBurst:
                PlasmaBurst(abilityTools);
                break;
            case AllAbilities.ChargeField:
                ChargeField(abilityTools);
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
        StartCoroutine(EnableForTime(abilityTools.duration, abilityTools.gameObject, abilityTools));
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

    private void RapidFire(AbilityTools abilityTools) {
        // RapidFire is just always enabled, no need to use EnableForTime coroutine
        // Spawn a bullet and fire it off
        Transform abilityToolsTransform = abilityTools.transform;
        Vector3 abilityToolsOldPos = abilityToolsTransform.position;
        abilityToolsTransform.LookAt(abilityTools.GetAim());
        abilityToolsTransform.Translate(Vector3.forward * 2f);
        GameObject bullet = Instantiate(abilityTools.abilityPrefab, abilityToolsTransform.position, abilityToolsTransform.rotation);
        abilityToolsTransform.position = abilityToolsOldPos;
        bullet.GetComponent<PlayerProjectile>().Go();
    }

    private void Electrorang(AbilityTools abilityTools, AbilityCooldowns.AbilitySlots slot) {
        // Instantiate Electrorang and position it slightly in front of the player
        Transform abilityToolsTransform = abilityTools.transform;
        GameObject electrorang = Instantiate(abilityTools.abilityPrefab, abilityToolsTransform.position, abilityToolsTransform.rotation);
        electrorang.transform.LookAt(abilityTools.GetAim());
        electrorang.transform.Translate(Vector3.forward * 2f);
        // Move it to aimed location and back
        electrorang.GetComponent<Electrorang>().FireElectrorang(gameObject, abilityTools, cooldowns, slot);
    }

    private void PlasmaBurst(AbilityTools abilityTools) {
        // Play the effect
        abilityTools.IncrementActivationId();
        ParticleSystem ps = abilityTools.ActivateParticleSystem();
        // Set potential colliders to all enemies
        int colliderCount = ps.trigger.colliderCount;
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        int i;
        for (i = 0; i < enemies.Count; i++) {
            if (i < colliderCount)
                ps.trigger.SetCollider(i, enemies[i].GetComponent<Collider>());
            else
                ps.trigger.AddCollider(enemies[i].GetComponent<Collider>());
        }

        if (i < colliderCount) {
            for (int y = colliderCount - 1; y >= i; y--) {
                ps.trigger.RemoveCollider(y);
            }
        }
    }

    private void ChargeField(AbilityTools abilityTools) {
        Vector3 aimedPosition = abilityTools.GetAim();
        Vector3 projectileSpawnPosition = Vector3.MoveTowards(transform.position, aimedPosition, 2f);
        GameObject chargeFieldProjectile = Instantiate(abilityTools.abilityPrefab, projectileSpawnPosition, Quaternion.identity);
        ChargeField chargeField = chargeFieldProjectile.GetComponent<ChargeField>();
        chargeField.Setup(abilityTools);
        // yeet
        chargeField.ThrowProjectile(aimedPosition);
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

    public void AbilityCooldownsReady() {
        // AbilityCooldowns calls this when it has finished loading its icons, etc...
        if (abilityCooldownsLoaded)
            return;
        cooldowns = GameObject.FindGameObjectWithTag("CooldownsUI").GetComponent<AbilityCooldowns>();
        abilityCooldownsLoaded = true;
    }
}
