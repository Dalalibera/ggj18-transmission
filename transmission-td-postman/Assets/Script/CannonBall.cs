using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {

    Transform target;
    Vector3 targetPosition;

    public float speed = 30f;
    public float radius = 1.5f;
    public GameObject impactEffect;
    public int damage;


    public void Seek(Transform _target, int damage) {
        target = _target;
        targetPosition = _target.position;
        this.damage = damage;
    }

    // Update is called once per frame
    void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = targetPosition - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) {
            dir = target.position - transform.position;
            if (dir.magnitude <= radius) {
                HitTarget(target, true);
            }
            else {
                HitTarget(target, false);
            }
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget(Transform t, bool insideRadius) {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);
        Destroy(gameObject);
        if (insideRadius) {
            Damage(t);
        }
    }

    void Damage(Transform enemy) {
        SpaceGhostMoviment en = enemy.GetComponent<SpaceGhostMoviment>();
        if (en != null) {
            en.TakeDamage(damage);
        }
    }

    void OnCollisionEnter(Collision collision) {
        HitTarget(collision.transform, true);
    }
}
