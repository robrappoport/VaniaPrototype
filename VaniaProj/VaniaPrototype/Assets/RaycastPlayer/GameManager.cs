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
    public int currentCharge = 0;
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
            //here is where we mark what upgrades the player has enabled
            //here is where we mark which charge station the player last touched
            //here is where we mark how much time the player has left
        }
   
    }

    // Update is called once per frame
    void Update()
    {
        currentCharge = Mathf.Clamp(currentCharge, 0, 1000000000);

    }

    public void EndGame()
    {
      
        SceneManager.LoadScene("GameOverScene");
    }

    public void WonGame()
    {

        SceneManager.LoadScene("GameWonScreen");
    }
}