using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {
    public GameObject completeLevelUI;

    public Transform zombiePoint1;
    public Transform zombiePoint2;
    public Transform zombiePoint3;
    public Transform zombiePoint4;
    public InputField itext;
    public float randomRange;
    public Text txt;
    public GameObject canvas;
    private bool btmClicked;
    private System.String nome;

    public Dictionary<string,float> highScore = new Dictionary<string,float>();

    private Vector3 offset;

    public bool isRunning;
    public float timer;

    private void Start() {
        btmClicked = false;
        Load();
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
        System.String key = null;
        foreach (System.String item in highScore.Keys) {
            if (maxValue<highScore[item]) {
                maxValue = highScore[item];
                key = item;
            }
        }
        if (highScore.Count >= 10) {
            if (maxValue > score && key != null) {
                highScore.Remove(key);
            } else {
                return;
            }
        }
        canvas.SetActive(true);

        if (!btmClicked){
            nome = "AAA";
        }

        highScore.Add(nome, score);
    }

    public void teste() {
        nome = itext.text;
        btmClicked = !btmClicked;
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/highScore.gd");
        bf.Serialize(file, highScore);
        file.Close();
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/highScore.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
    //        SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
    }
}
