using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttack : EnemyAttacks
{
    [Tooltip("Prefab that serves as the projectile")]
    public GameObject projectile;
    [Tooltip("The angle the projectile is shot at")]
    public float projectileArc;
    [Tooltip("The speed the projectile is shot at")]
    public float projectileSpeed;

    // override the attack method
    public override void performAttack(Transform player, NavMeshAgent enemy)
    {
        // Creates object to shot and spawn it in front of the enemy
        GameObject projectileObject = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
        Rigidbody rb = projectileObject.GetComponent<Rigidbody>();
        // Adds velocity to projectile
        // Variables may be adjusted for accurate shooting
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        rb.AddForce(transform.up * projectileArc, ForceMode.Impulse);
    }
}
