using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : EnemyAttacks
{
    // override the attack method
    public override void performAttack(Transform player, NavMeshAgent enemy)
    {
        // ignore any collision besides the player and enable the box collider
        GetComponent<Collider>().enabled = true;
        // wait and then disable the box collider
        StartCoroutine(waitAndDisable());
    }
    IEnumerator waitAndDisable()
    {
        // wait for 1 second
        yield return new WaitForSeconds(1);
        // disable the box collider
        GetComponent<Collider>().enabled = false;
    }
    
}