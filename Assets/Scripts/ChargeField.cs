using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class ChargeField : MonoBehaviour {
    public GameObject indicator;
    public GameObject hitboxObj;
    public GameObject lightningPrefab;

    public float thrownForce;

    private AbilityTools abilityTools;
    private Rigidbody rigidBody;
    private bool isThrown;
    private bool isDeployed;
    private float timeLeft;
    private Hitbox hitbox;
    private PlayerStats playerStats;
    
    private const float LightningFrequency = 0.4f;
    private const float LightningLifespan = 0.2f;
    private void Update() {
        if (isDeployed && timeLeft > 0f) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f) {
                isDeployed = false;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (isThrown) {
            // Stop moving and deploy
            rigidBody.isKinematic = true;
            isThrown = false;
            DeployChargeField();
        }
    }

    public void Setup(AbilityTools tools) {
        isThrown = false;
        isDeployed = false;
        rigidBody = gameObject.GetComponent<Rigidbody>();
        abilityTools = tools;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void ThrowProjectile(Vector3 towards) {
        // Use InverseTransformPoint to get a direction vector relative to our position
        rigidBody.AddRelativeForce(transform.InverseTransformPoint(towards).normalized * thrownForce);
        isThrown = true;
    }

    private void DeployChargeField() {
        Debug.Log("Deploying Charge Field!");
        indicator.SetActive(true);
        isDeployed = true;
        timeLeft = abilityTools.duration;
        hitboxObj.SetActive(true);
        hitbox = hitboxObj.GetComponent<Hitbox>();
        hitbox.collectStats = true;
        StartCoroutine(ActivateLightning());
    }

    private IEnumerator ActivateLightning() {
        while (isDeployed) {
            yield return new WaitForSeconds(LightningFrequency);
            PulseLightning();
        }
    }
    
    private void PulseLightning() {
        foreach (EnemyAI enemy in hitbox.collisions) {
            if (enemy == null) continue;
            GameObject lightning = Instantiate(lightningPrefab, transform);
            LightningBoltScript lbs = lightning.GetComponent<LightningBoltScript>();
            lbs.StartPosition = transform.position;
            lbs.EndPosition = enemy.transform.position;
            Destroy(lightning, LightningLifespan);
            // And do damage
            enemy.TakeDamage(abilityTools.damage);
            // And apply speed debuff
            hitbox.collisionsStats[enemy].activeBuffs[BaseStats.Buffs.SpeedBuff] = new Vector3(.4f, .25f);
        }

        if (hitbox.playerInHitbox) {
            // Buff player speed
            playerStats.activeBuffs[BaseStats.Buffs.SpeedBuff] = new Vector3(.4f, 2f);
        }
    }
}
