using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour {
    public GameManager gameManager;

    void OnTriggerEnter() {
        gameManager.CompleteGame();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
