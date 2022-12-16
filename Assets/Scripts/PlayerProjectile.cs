using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    public float lifespan;
    public int damage;
    public float force;
    public Rigidbody rb;

    private Vector3 lastPos;

    public void Go(int newDamage) {
        damage = newDamage;
        rb.AddForce(transform.forward * force);
        lastPos = transform.position;
    }

    private void Update() {
        KeyValuePair<bool, EnemyAI> hitLastFrame = CheckHitLastFrame();
        if (hitLastFrame.Key) {
            if (hitLastFrame.Value != null)
                hitLastFrame.Value.TakeDamage(damage);
            Destroy(gameObject);
        }
        else {
            lifespan -= Time.deltaTime;
            if (lifespan <= 0f)
                Destroy(gameObject);
        }

        lastPos = transform.position;
    }

    private KeyValuePair<bool, EnemyAI> CheckHitLastFrame() {
        if (Physics.Raycast(lastPos, transform.position - lastPos, out RaycastHit hit, Vector3.Distance(transform.position, lastPos))) {
            if (hit.transform.gameObject.CompareTag("Enemy"))
                return new KeyValuePair<bool, EnemyAI>(true, hit.transform.gameObject.GetComponent<EnemyAI>());
            if (hit.transform.gameObject.CompareTag("Player"))
                return new KeyValuePair<bool, EnemyAI>(false, null);
            return new KeyValuePair<bool, EnemyAI>(true, null);
        }

        return new KeyValuePair<bool, EnemyAI>(false, null);
    }
}
