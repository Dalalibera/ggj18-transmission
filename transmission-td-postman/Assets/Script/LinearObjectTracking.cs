using UnityEngine;
using System.Collections;

/**
 * Usado para seguir o player (camera ou whatever)
 */
public class LinearObjectTracking : MonoBehaviour
{

    private Vector3 startPos;
    public GameObject target;

    void Awake()
    {
        if(target != null)
            startPos = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            transform.position = target.transform.position + startPos;
    }
}
