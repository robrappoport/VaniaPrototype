using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float rand;
    public float randCap;
    public GameObject coin;
    public Vector2 [] spawnLoc;
    public bool coinExists;
    public int score = 0;
    public int highScore = 0;
    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Ensure we leave start if we're destroying ourself (Destroy actually just marks us as "to be destroyed soon").
        }
        print(Application.persistentDataPath);
        string fullFilePath = Application.dataPath + Path.DirectorySeparatorChar + "SaveData.txt";
        if (File.Exists(fullFilePath))
        {
            string highScoreString = File.ReadAllText(fullFilePath);
            highScore = int.Parse(highScoreString);
        }
        rand = 0f;
        randCap = Random.Range(1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.Clamp(score, 0, 1000000000);
        if (!coinExists)
        {
            rand += Time.deltaTime;
        }
        

        if (rand >= randCap)
        {
            coinExists = true;
            GameObject c = Instantiate(coin) as GameObject;
            int rando = Random.Range(0, spawnLoc.Length);
            c.transform.position = spawnLoc[rando];
            randCap = Random.Range(1f, 3f);
            rand = 0f;
        }

    }

    public void EndGame()
    {
        if (score > highScore)
        {
            highScore = score;

            string fullFilePath = Application.dataPath + Path.DirectorySeparatorChar + "SaveData.txt";
            File.WriteAllText(fullFilePath, highScore.ToString());
        }
        SceneManager.LoadScene("GameOverScene");
    }
}