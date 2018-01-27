using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowWaypoints : MonoBehaviour {

    public Transform[] Waypoints;

    private enum State
    {
        WAIT
        , WALK
    }

    private State state = State.WAIT;
    private NavMeshAgent navAgent;

    private int current = 0;
    private int wait = 0;
    private Transform target;

    // Use this for initialization
    void Start () {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Waypoints.Length == 0)
            return;

        switch (state)
        {
            case State.WAIT:
                if (wait-- <= 0)
                {
                    Enter(State.WALK);
                }
                break;
            case State.WALK:
                if (navAgent.remainingDistance < 0.1f && Vector3.Distance(navAgent.destination, transform.position) < 0.5f)
                {
                    Enter(State.WAIT);
                }
                break;
        }
    }

    private void Enter(State state) {
        switch(state)
        {
            case State.WAIT:
                wait = 20;
                break;
            case State.WALK:
                current = (current + 1) % Waypoints.Length;
                navAgent.destination = Waypoints[current].position;
                Debug.Log(Waypoints[current].name);
                break;
        }
        this.state = state;
    }
}
