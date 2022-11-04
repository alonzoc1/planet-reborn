using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public GameObject physicalBullet;
    public float speed;
    public float maxLifespan;
    public Vector3 goTo;

    private float lastDistance;
    private float lastDirection;
    private Boolean firstTick;
    private TrailRenderer trail;
    private void Start() {
        Destroy(gameObject, maxLifespan);
        firstTick = true;
        trail = gameObject.GetComponent<TrailRenderer>();
    }
    
    private void Update() {
        // Destroy projectile when it arrives at target point (later we can get fancy with destroy animations)
        float newDistance = Vector3.Distance(gameObject.transform.position, goTo);
        float newDirection = Vector3.Angle(gameObject.transform.position, goTo);
        if (!firstTick && (newDistance >= lastDistance || Math.Abs(newDirection - lastDirection) >= 45)) {
            Destroy(physicalBullet);
            trail.emitting = false;
        }

        firstTick = false;

        lastDirection = newDirection;
        lastDistance = newDistance;
        gameObject.transform.LookAt(goTo);
        gameObject.transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
}
