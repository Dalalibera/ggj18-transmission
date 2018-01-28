using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UpdateSpeed : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float move = Input.GetAxis("Vertical");
        animator.SetFloat("Speed", move);
	}
}
