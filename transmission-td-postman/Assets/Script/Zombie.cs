using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {
    Animator animator;

    public Transform player;

    private NavMeshAgent agent;
    public float range = 1000f;
    private SpaceGhostMoviment en;

    void Start() {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
        en = player.GetComponent<SpaceGhostMoviment>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);

        if (en.isAlive()) {
            if (distanceToEnemy <= range) {
                animator.SetBool("PlayerInRange", true);
                agent.SetDestination(player.position);
            }
            else {
                animator.SetBool("PlayerInRange", false);
            }
        }
    }

    private void OnCollisionEnter() {
        if (en != null) {
            en.TakeDamage(100);
            animator.SetTrigger("PlayerIsDead");
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
