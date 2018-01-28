using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTime : MonoBehaviour {
    public GameManager gameManager;
    public int NumberOfZombies;
    public GameObject ZombiePrefab;
    bool isTriggered = false;

    private void OnTriggerEnter(Collider other) {
        if (!isTriggered) {
            gameManager.Zombies(other.transform, ZombiePrefab, NumberOfZombies);
            isTriggered = true;
        }
    }
}
