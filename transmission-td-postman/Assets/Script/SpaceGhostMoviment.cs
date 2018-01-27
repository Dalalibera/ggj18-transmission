using UnityEngine;

public class SpaceGhostMoviment : MonoBehaviour {

    public float horizontalSpeed = 150f;
    public float verticalSpeed = 2f;

    Animator animator;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * horizontalSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * verticalSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
            animator.SetTrigger("Jumping");

        if (Input.GetKeyDown(KeyCode.LeftControl))
            animator.SetTrigger("Slidding");

        if (Input.GetKeyDown(KeyCode.F))
            animator.SetTrigger("Death");

    }

}
