using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject completeLevelUI;

    public void CompleteGame() {
        completeLevelUI.SetActive(true);
    }
}
