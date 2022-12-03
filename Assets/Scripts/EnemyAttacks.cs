using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttacks : MonoBehaviour
{
    private EnemyAI enemyAI;

    public virtual void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }
    public virtual void performAttack(Transform player, NavMeshAgent enemy)
    {
        
    }
}
