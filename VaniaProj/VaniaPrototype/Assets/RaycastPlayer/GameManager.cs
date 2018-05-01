using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using SimpleJSON;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int currentCharge = 0;
    public static List<ChargerTypeScript> chargerList;
    public GameObject playerPrefab;
    private Transform gameStartLoc;
    private GameObject player;
    public GameObject levelHolder;
    public ChargerTypeScript currentActiveCharger;
    [Header("Upgrades Enabled")]
    public bool doubleJumpEnabled = false;
    public bool wallJumpEnabled = false;
    public bool airDashEnabled = false;

    public Transform[] upgradeSpawners;
    public GameObject[] Upgrades;
    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            instance.Start();
            return; // Ensure we leave start if we're destroying ourself (Destroy actually just marks us as "to be destroyed soon").
        }

        chargerList = new List<ChargerTypeScript>(levelHolder.GetComponentsInChildren<ChargerTypeScript>());
        gameStartLoc = GameObject.Find("GAME STARTO").transform;
       

    }

    public void Start()
	{
        if (File.Exists(Application.dataPath + Path.DirectorySeparatorChar + "currentGameState.json"))
        {
            LoadLevel();  
        }
        else
        {
            player = Instantiate(playerPrefab, gameStartLoc.position, Quaternion.identity);
            Camera.main.GetComponent<CameraFollow>().target = player;
        }
       
        if (doubleJumpEnabled == false)
        {
            Instantiate(Upgrades[0], upgradeSpawners[0]);
        }
        if (wallJumpEnabled == false)
        {
            Instantiate(Upgrades[1], upgradeSpawners[1]);
        }
        if (airDashEnabled == false)
        {
            Instantiate(Upgrades[2], upgradeSpawners[2]);
        }
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            SaveLevel();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            SaveLevel();
            SceneManager.LoadScene("MainScene");
        }

    }

    public void EndGame()
    {
        SaveLevel();
        SceneManager.LoadScene("GameOverScene");
    }

    public void WonGame()
    {

        SceneManager.LoadScene("GameWonScreen");
    }

    public void AssignActiveCharger(ChargerTypeScript charger)
    {
        currentActiveCharger = charger;
    }

    public void LoadLevel()
    {
        string filename = Application.dataPath + Path.DirectorySeparatorChar + "currentGameState.json";
        string levelJSONString = File.ReadAllText(filename);

        //load character location
        JSONObject levelJSON = (JSONObject)JSON.Parse(levelJSONString);
        JSONObject activeChargerJSON = levelJSON["playerSpawnPoint"].AsObject;
        float playerLocX = activeChargerJSON["x"].AsFloat;
        float playerLocY = activeChargerJSON["y"].AsFloat;
        player = Instantiate(playerPrefab, new Vector2(playerLocX, playerLocY), Quaternion.identity);
        doubleJumpEnabled = levelJSON["doubleJumpActive"].AsBool;
        wallJumpEnabled = levelJSON["wallJumpActive"].AsBool;
        airDashEnabled = levelJSON["airDashActive"].AsBool;

        player.GetComponent<RigidbodyPlayer>().smallMaxCharge = levelJSON["currentlyAvailableCharge"].AsFloat;

        Camera.main.GetComponent<CameraFollow>().target = player;

    }
    public void SaveLevel()
    {
        JSONObject levelJSON = new JSONObject();

        //saving the current active charger to set spawn point
        JSONObject activeChargerJSON = new JSONObject();
        activeChargerJSON["x"].AsFloat = currentActiveCharger.gameObject.transform.position.x;
        activeChargerJSON["y"].AsFloat = currentActiveCharger.gameObject.transform.position.y + .5f;
        levelJSON["playerSpawnPoint"] = activeChargerJSON;

        //saving what upgrades are currently obtained
        JSONBool dJumpJSON = new JSONBool(doubleJumpEnabled);
        JSONBool wJumpJSON = new JSONBool(wallJumpEnabled);
        JSONBool aDashJSON = new JSONBool(airDashEnabled);
        levelJSON["doubleJumpActive"] = dJumpJSON;
        levelJSON["wallJumpActive"] = wJumpJSON;
        levelJSON["airDashActive"] = aDashJSON;

        //saving smallMaxCharge
        JSONNumber chargeJSON = new JSONNumber(player.GetComponent<RigidbodyPlayer>().smallMaxCharge);
        levelJSON["currentlyAvailableCharge"] = chargeJSON;

        string levelJSONString = levelJSON.ToString();
        string filename = Application.dataPath + Path.DirectorySeparatorChar + "currentGameState.json";
        Debug.Log(levelJSONString);
        File.WriteAllText(filename, levelJSONString);
    }

	private void OnApplicationQuit()
	{
        SaveLevel();
	}


}