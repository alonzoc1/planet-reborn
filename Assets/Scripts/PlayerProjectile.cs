using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {
    public float lifespan;
    public int damage;
    public float force;
    public Rigidbody rb;

    public void Go() {
        rb.AddForce(transform.forward * force);
    }

    private void Update() {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Bullet collided");
        Destroy(gameObject);
    }
}
