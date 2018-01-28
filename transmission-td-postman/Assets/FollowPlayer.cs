using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offset;
    public Vector3 playerDir;
    public Vector3 playerPrevPos;
    public float distance;

    void LateUpdate() {
        float desiredAngle = player.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle - 90, 0);
        transform.position = player.transform.position - (rotation * offset);
        transform.LookAt(player.position + new Vector3(0, 3, 0));
    }
}
