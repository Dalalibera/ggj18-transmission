using UnityEngine;
using UnityEngine.UI;

public class SpaceGhostMoviment : MonoBehaviour {

    public float horizontalSpeed = 150f;
    public float verticalSpeed = 2f;
    public float startHealth = 100;
    private float health;
    public Image healthBar;
    private bool alive;
    float lockPos = 0;
    public GameManager gm;
    Vector3 oldPos;
    Vector3 identity = new Vector3(1, 0, 1);

    Animator animator;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        health = startHealth;
        this.alive = true;
        oldPos = Vector3.Scale(oldPos, identity);
    }

    // Update is called once per frame
    void Update() {
        if (gm != null) {
            if (!gm.isRunning && Vector3.Scale(transform.position, identity) != oldPos) {
                gm.isRunning = true;
            }
        }

        if (this.alive) {
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * horizontalSpeed;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * verticalSpeed;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);

            if (Input.GetKeyDown(KeyCode.Space)) {
                animator.SetTrigger("Jumping");
                GetComponent<Rigidbody>().velocity = new Vector3(0, 10, 0);
            }


            if (Input.GetKeyDown(KeyCode.LeftControl))
                animator.SetTrigger("Slidding");

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                animator.SetTrigger("Backward");

        }

        transform.rotation = Quaternion.Euler(lockPos, transform.rotation.eulerAngles.y, lockPos);

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
