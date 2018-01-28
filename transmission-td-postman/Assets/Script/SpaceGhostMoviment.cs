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
        this.alive = true;
    }

    // Update is called once per frame
    void Update() {
        if (this.alive) {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * horizontalSpeed;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * verticalSpeed;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);

            if (Input.GetKeyDown(KeyCode.Space))
                animator.SetTrigger("Jumping");

            if (Input.GetKeyDown(KeyCode.LeftControl))
                animator.SetTrigger("Slidding");

            if (Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))
                animator.SetTrigger("Backward");

        }

        if (health <= 0 && alive)
            Die();
    }

    public bool isAlive() {
        return this.alive;
    }

    public void TakeDamage(int amount) {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
    }

    public void Die() {
        this.alive = false;
        animator.SetTrigger("Death");
    }

}
