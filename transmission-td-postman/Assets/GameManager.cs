using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject completeLevelUI;

    public Transform zombiePoint1;
    public Transform zombiePoint2;
    public Transform zombiePoint3;
    public Transform zombiePoint4;
    public InputField itext;
    public float randomRange;
    public Text txt;
    public Text hiScore;
    public GameObject canvas;
    private bool btmClicked;
    private System.String nome;

    public Dictionary<float, System.String> highScoreDict = new Dictionary<float, System.String>();

    private Vector3 offset;

    public bool isRunning;
    public float timer;

    private void Start() {
        btmClicked = false;
        Load();

        foreach (float item in highScoreDict.Keys){
            float minValue = float.MaxValue;
            foreach (float score in highScoreDict.Keys){
                if (minValue > score){
                    minValue = score;
                }

            }

            hiScore.text += highScoreDict[item] + ": " + item + " \n";
            highScoreDict.Remove(minValue);
        }

        isRunning = false;
        timer = 0f;
    }

    private void Update() {
        if (isRunning) {
            txt.text = timer.ToString("F2") + " seconds";
            timer += Time.deltaTime;
        }
    }

    public void CompleteGame() {
        isRunning = false;
        completeLevelUI.SetActive(true);
    }

    public void Zombies(Transform target, GameObject zombie, int zombies) {

        for (int i = 0; i < zombies; i++) {
            offset = new Vector3(Random.Range(-randomRange, randomRange), 0, Random.Range(-randomRange, randomRange));

            GameObject zombieGO = (GameObject)Instantiate(zombie, zombiePoint1.position + offset, zombiePoint1.rotation);
            Zombie zomb1 = zombieGO.GetComponent<Zombie>();
            zomb1.Seek(target);

            offset = new Vector3(Random.Range(-randomRange, randomRange), 0, Random.Range(-randomRange, randomRange));
            zombieGO = (GameObject)Instantiate(zombie, zombiePoint2.position + offset, zombiePoint2.rotation);
            Zombie zomb2 = zombieGO.GetComponent<Zombie>();
            zomb2.Seek(target);

            offset = new Vector3(Random.Range(-randomRange, randomRange), 0, Random.Range(-randomRange, randomRange));
            zombieGO = (GameObject)Instantiate(zombie, zombiePoint3.position + offset, zombiePoint3.rotation);
            Zombie zomb3 = zombieGO.GetComponent<Zombie>();
            zomb3.Seek(target);

            offset = new Vector3(Random.Range(-randomRange, randomRange), 0, Random.Range(-randomRange, randomRange));
            zombieGO = (GameObject)Instantiate(zombie, zombiePoint4.position + offset, zombiePoint4.rotation);
            Zombie zomb4 = zombieGO.GetComponent<Zombie>();
            zomb4.Seek(target);

        }
    }

    public void HighScore(float score) {

        float maxValue = 0f;
        float key = 0;
        foreach (float item in highScoreDict.Keys) {
            if (maxValue<item) {
                maxValue = item;
                key = item;
            }
        }
        if (highScoreDict.Count >= 10) {
            if (maxValue > score && key > 0) {
                highScoreDict.Remove(key);
            } else {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return;
            }
        }
        canvas.SetActive(true);
    }

    public void teste() {
        nome = itext.text;
        Debug.Log(highScoreDict.Count);
        btmClicked = !btmClicked;
        highScoreDict.Add(timer, nome);
        Debug.Log(highScoreDict.Count);
        Save();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/highScore.gd");
        bf.Serialize(file, highScoreDict);
        file.Close();
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/highScore.gd")) {
            Debug.Log(Application.persistentDataPath + "/highScore.gd");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/highScore.gd", FileMode.Open);
            highScoreDict = (Dictionary<float, System.String>)bf.Deserialize(file);
            file.Close();
        }
    }
}
