using UnityEngine;

public class LevelComplete : MonoBehaviour {

    public GameManager gm;

    public void LoadNextLevel() {
        Time.timeScale = 0;
        if(gm != null){
            gm.HighScore(gm.timer);
        }        
    }
}
