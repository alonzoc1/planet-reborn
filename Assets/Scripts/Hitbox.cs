using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    public HashSet<EnemyAI> collisions;
    public Dictionary<EnemyAI, EnemyStats> collisionsStats;
    public bool playerInHitbox;
    public bool collectStats;

    private void Start() {
        collisions = new HashSet<EnemyAI>();
        collisionsStats = new Dictionary<EnemyAI, EnemyStats>();
        playerInHitbox = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            EnemyAI ai = other.gameObject.GetComponent<EnemyAI>();
            collisions.Add(ai);
            if (collectStats)
                collisionsStats[ai] = other.gameObject.GetComponent<EnemyStats>();
        }

        if (other.gameObject.CompareTag("Player"))
            playerInHitbox = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            EnemyAI ai = other.gameObject.GetComponent<EnemyAI>();
            collisions.Remove(ai);
            if (collectStats)
                collisionsStats.Remove(ai);
        }

        if (other.gameObject.CompareTag("Player"))
            playerInHitbox = false;
    }
}
