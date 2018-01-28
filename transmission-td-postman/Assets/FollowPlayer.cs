using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offset;
    public Vector3 playerDir;
    public Vector3 playerPrevPos;
    public float distance;

    void Start() {
        /*offset = transform.position - player.transform.position;
        distance = offset.magnitude;
        playerPrevPos = player.transform.position;        */
    }


    void LateUpdate() {
        transform.position = player.position + offset;
        /*ayerDir = player.transform.position - playerPrevPos;

        playerDir.Normalize();
        if (playerDir.x!=0) {
            transform.position = player.transform.position - playerDir * distance;
        }

        transform.LookAt(player.transform.position);
        playerPrevPos = player.transform.position;*/
    }
}
