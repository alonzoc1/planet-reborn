using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class Electrorang : MonoBehaviour {
    public GameObject lightningPrefab;
    public Hitbox hitbox;
    
    private GameObject player;
    private AbilityCooldowns cooldowns;
    private AbilityCooldowns.AbilitySlots slotToReady;
    private AbilityTools tools;
    private bool isFired;
    private bool isReturning;
    private Vector3 destination;
    private float speed;
    private float lifespan;

    private const float LightningFrequency = 0.2f;
    private const float LightningLifespan = 0.15f;

    private void Start() {
        player = GameObject.FindWithTag("Player");
        hitbox.collectStats = false;
    }

    private void Update() {
        float distanceToTravel;
        if (!isFired)
            return;
        if (!isReturning) {
            // Fly to location
            distanceToTravel = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination, distanceToTravel);
            if (transform.position.Equals(destination))
                isReturning = true;
            lifespan -= Time.deltaTime;
            if (lifespan <= 0f) {
                isReturning = true;
            }
        }
        else {
            // Return to player and deactivate self
            Vector3 playerLocation = player.transform.position;
            distanceToTravel = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerLocation, distanceToTravel);
            if (transform.position.Equals(playerLocation)) {
                cooldowns.ManualReadyAbility(slotToReady);
                gameObject.SetActive(false);
            }
        }
    }

    public void FireElectrorang(GameObject playerObj, AbilityTools electrorangTools, AbilityCooldowns cds, AbilityCooldowns.AbilitySlots slot) {
        isFired = true;
        isReturning = false;
        destination = electrorangTools.GetAim();
        speed = electrorangTools.speed;
        player = playerObj;
        tools = electrorangTools;
        cooldowns = cds;
        slotToReady = slot;
        // For Electrorang, cooldown is how long the ball can travel TOWARDS the destination
        lifespan = electrorangTools.cooldown;
        StartCoroutine(ActivateLightning());
    }

    IEnumerator ActivateLightning() {
        while (isFired) {
            yield return new WaitForSeconds(LightningFrequency);
            PulseLightning();
        }
    }

    private void PulseLightning() {
        GameObject lightning;
        LightningBoltScript lbs;
        foreach (EnemyAI enemy in hitbox.collisions) {
            if (enemy == null) continue;
            lightning = Instantiate(lightningPrefab, transform);
            lbs = lightning.GetComponent<LightningBoltScript>();
            lbs.StartPosition = transform.position;
            lbs.EndPosition = enemy.transform.position;
            Destroy(lightning, LightningLifespan);
            // And do damage
            enemy.TakeDamage(tools.damage);
        }
    }
}
