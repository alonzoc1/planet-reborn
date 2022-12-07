using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    public HashSet<EnemyAI> collisions;

    private void Start() {
        collisions = new HashSet<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy"))
            collisions.Add(other.gameObject.GetComponent<EnemyAI>());
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Enemy"))
            collisions.Remove(other.gameObject.GetComponent<EnemyAI>());
    }
}
