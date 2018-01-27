using UnityEngine;

public class Bullet : MonoBehaviour {

    Transform target;

    public float speed = 7f;
    public GameObject impactEffect;
    public int damage = 25;


    public void Seek(Transform _target) {
        target = _target;
    }

    // Update is called once per frame
    void Update() {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) {
            HitTarget(target);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget(Transform t) {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);
        Destroy(gameObject);

        Damage(t);
    }

    void Damage(Transform enemy) {
        SpaceGhostMoviment en = enemy.GetComponent<SpaceGhostMoviment>();
        if (en!=null) {
            en.TakeDamage(damage);
        }
    }
}
