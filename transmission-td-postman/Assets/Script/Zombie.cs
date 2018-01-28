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
        agent.speed = Random.Range(2.5f, 4.5f);
        animator.speed = agent.speed / 3.5f;
    }

    public void Seek(Transform _target) {
        player = _target;
    }

    private void Update() {
        if (en.isAlive()) {
            float distanceToEnemy = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToEnemy <= range) {
                animator.SetBool("PlayerInRange", true);
                agent.SetDestination(player.position);
            }
            else {
                animator.SetBool("PlayerInRange", false);
            }
        }
        else {
            agent.SetDestination(transform.position);
            transform.LookAt(player.transform);
            animator.SetTrigger("PlayerIsDead");
        }
    }

    private void OnCollisionEnter() {
        if (en != null) {
            en.TakeDamage(100);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
