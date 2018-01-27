using UnityEngine;
using UnityEngine.UI;

public class SpaceGhostMoviment : MonoBehaviour {

    public float horizontalSpeed = 150f;
    public float verticalSpeed = 2f;
    public float startHealth = 100;
    private float health;
    public Image healthBar;
    private bool alive;

    Animator animator;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        health = startHealth;
        alive = true;
    }

    // Update is called once per frame
    void Update() {
        if (alive) {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * horizontalSpeed;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * verticalSpeed;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);

            if (Input.GetKeyDown(KeyCode.Space))
                animator.SetTrigger("Jumping");

            if (Input.GetKeyDown(KeyCode.LeftControl))
                animator.SetTrigger("Slidding");
        }

    }

    public bool isAlive() {
        return alive;
    }

    public void TakeDamage(int amount) {
        health -= amount;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
            Die();
    }

    public void Die() {
        animator.SetTrigger("Death");
        alive = false;
    }

}
