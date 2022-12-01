using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class EnemyAI : MonoBehaviour
{
    [Tooltip("Prefab that serves as the projectile")]
    public GameObject projectile;
    [Tooltip("Set appropriate layer respectively")]
    public LayerMask whatIsGround, whatIsPlayer;
    [Tooltip("How far the AI can search at a time")]
    public float walkPointRange;
    // Searching State
    private Vector3 walkPoint;
    private bool walkPointSet;
    // GameObjects
    private Transform player;
    private NavMeshAgent enemy;
    private EnemyStats stats;
    // Variables
    private bool playerInSight, playerInAttackRange;
    private bool isAttacking;
    [Tooltip("The angle the projectile is shot at")]
    public float projectileArc;
    [Tooltip("The speed the projectile is shot at")]
    public float projectileSpeed;
    private Dictionary<PlayerAbilities.AllAbilities, int> recentDamageTaken;
    private Dictionary<PlayerAbilities.AllAbilities, AbilityTools> collidingWith;
    private float damageTimeBuffer;

    private void Awake() {
        recentDamageTaken = new Dictionary<PlayerAbilities.AllAbilities, int>();
        collidingWith = new Dictionary<PlayerAbilities.AllAbilities, AbilityTools>();
        damageTimeBuffer = 0f;
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
        stats = GetComponent<EnemyStats>();
        enemy.speed = stats.movementSpeed;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSight = Physics.CheckSphere(transform.position, stats.sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, stats.attackRange, whatIsPlayer);

        if (!playerInSight && !playerInAttackRange) Searching();
        if (playerInSight && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSight) AttackPlayer();

        // Deal one tick of colliding multi-hit damage 5 times a second
        damageTimeBuffer += Time.deltaTime;
        while (damageTimeBuffer >= .2f) { // Should only loop once per .2 seconds (less than once a frame typically)
            damageTimeBuffer -= .2f;
            List<PlayerAbilities.AllAbilities> toRemove = new List<PlayerAbilities.AllAbilities>();
            foreach (KeyValuePair<PlayerAbilities.AllAbilities, AbilityTools> damageSource in collidingWith) {
                if (!damageSource.Value.gameObject.activeInHierarchy)
                    // OnTriggerExit sometimes won't trigger if ability runs out of time before physically exiting the
                    // enemy's collider, so we do an active check here
                    toRemove.Add(damageSource.Key);
                else
                    TakeDamage(damageSource.Value.damage);
            }

            foreach (PlayerAbilities.AllAbilities remove in toRemove)
                collidingWith.Remove(remove);
        }
    }

    private void Searching()
    {
        // Creates new walk point if there are no current ones
        if (!walkPointSet) SearchWalkPoint();
        // Walks towards walk point
        if (walkPointSet)
            enemy.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        // Checks if we have reached the walk point
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        // Checks to see if our new walk point is in a valid location
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        enemy.SetDestination(transform.position);
        transform.LookAt(player);

        if (isAttacking) return;
        // Creates object to shot
        Rigidbody rb = Instantiate(projectile, transform.position, transform.rotation).GetComponent<Rigidbody>();
        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>(),true);
        // Adds velocity to projectile
        // Variables may be adjusted for accurate shooting
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
        rb.AddForce(transform.up * projectileArc, ForceMode.Impulse);
        isAttacking = true;
        // Delays the next attack 
        Invoke(nameof(ResetAttack), stats.attackSpeed);
    }
    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider collision) {
        // This function detects when we're colliding with a damage source, and decides to apply damage
        // if needed. For multi-hit damage sources, we also store the ability name and continue taking damage
        // from it over time until OnTriggerExit removes the collision. This has better performance than OnTriggerStay
        if (collision.gameObject.CompareTag("DamageSource")) {
            AbilityTools abilityTools = collision.gameObject.GetComponent<AbilityTools>();
            if (!abilityTools.hitsOnlyOnce) {
                // Deal with multi-hit attacks in Update
                collidingWith.TryAdd(abilityTools.abilityName, abilityTools);
                return;
            }

            if (!CanTakeDamageFromSource(abilityTools))
                return;
        
            TakeDamage(abilityTools.damage);
            recentDamageTaken[abilityTools.abilityName] = abilityTools.activationId;
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (!collision.gameObject.CompareTag("DamageSource"))
            return;

        collidingWith.Remove(collision.GetComponent<AbilityTools>().abilityName);
    }

    private bool CanTakeDamageFromSource(AbilityTools abilityTools) {
        // If we're getting hit by the same instance of the same ability, ignore it
        return !(recentDamageTaken.ContainsKey(abilityTools.abilityName) &&
                 recentDamageTaken[abilityTools.abilityName] == abilityTools.activationId);
    }

    // Reduce the enemy's health, positive "damage" reduces health
    public void TakeDamage(int damage)
    {
        stats.ChangeCurrentHealth(damage * -1);
        if (stats.state == BaseStats.State.Dead)
            Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        stats.Cleanup();
        Destroy(gameObject);
    }
}
