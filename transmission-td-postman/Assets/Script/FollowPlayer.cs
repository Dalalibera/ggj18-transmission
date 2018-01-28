using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offset;
    public Vector3 playerDir;
    public Vector3 playerPrevPos;
    public float distance;
    float cameraCorrection = 0;
    
    void LateUpdate() {


        if (Input.GetMouseButton(0))
        {
            cameraCorrection += Input.GetAxis("Mouse Y");
        }

        float desiredAngle = player.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle - 90, 0);
        transform.position = player.transform.position - (rotation * offset);
        transform.LookAt(player.position + new Vector3(0, cameraCorrection, 0));
    }
}
