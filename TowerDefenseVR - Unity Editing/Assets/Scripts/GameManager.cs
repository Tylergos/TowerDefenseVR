using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawnerPrefab;

    [SerializeField]
    private GameObject bagPrefab;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject playerVRPrefab;
    
    private bool isVR;
    [SerializeField]
    private GameObject inputManager;

    private GameObject player;
    private CharacterScript charScript;
    private CharacterStats playerStats;
    private GameObject enemySpawner;

    private string vrModel;

    private void Awake()
    {
        if (OVRManager.isHmdPresent)
        {
            isVR = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("EnemySpawner") == null)
        {
            enemySpawner = Instantiate(enemySpawnerPrefab);
        }
        if (GameObject.FindGameObjectWithTag("Bag") == null)
        {
             Instantiate(bagPrefab);
        }
        if (GameObject.FindGameObjectWithTag("Player") == null && !isVR)
        {
            playerStats = new CharacterStats();
            player = Instantiate(playerPrefab, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.rotation);
            charScript = player.GetComponent<CharacterScript>();
            charScript.IsVR(isVR);
            charScript.SetStats(playerStats.speed, playerStats.maxHealth, playerStats.maxMana, playerStats.xpLevel, playerStats.maxLevelXP, playerStats.xp);
            enemySpawner.GetComponent<EnemySpawner>().SetPlayer(player);
        }
        else if (GameObject.FindGameObjectWithTag("Player") == null && isVR)
        {
            playerStats = new CharacterStats();
            player = Instantiate(playerVRPrefab, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.rotation);
            player.GetComponent<CharacterScript>().IsVR(isVR);
            enemySpawner.GetComponent<EnemySpawner>().SetPlayer(player);
            GameObject manager = Instantiate(inputManager);
            charScript.SetStats(playerStats.speed, playerStats.maxHealth, playerStats.maxMana, playerStats.xpLevel, playerStats.maxLevelXP, playerStats.xp);
            manager.GetComponent<InputManager>().SetPlayer(player);
        }
    }

    private CharacterStats CreatePlayerStatsObject()
    {
        CharacterStats charStats = new CharacterStats();
        Dictionary<string, float> stats = charScript.GetStats();
        charStats.speed = stats["speed"];
        charStats.maxHealth = (int)stats["maxhealth"];
        charStats.maxMana = (int)stats["maxmana"];
        charStats.xpLevel = (int)stats["level"];
        charStats.xp = (int)stats["xp"];
        return charStats;
    }

    public void SavePlayer()
    {
        CharacterStats charStats = CreatePlayerStatsObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerdata.data");

        bf.Serialize(file, charStats);
        file.Close();

        Debug.Log("Game Saved to: " + Application.persistentDataPath + "/playerdata.data");
    }

    public void LoadPlayer()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerdata.data", FileMode.Open);

        CharacterStats stats = (CharacterStats)bf.Deserialize(file);
        file.Close();

        charScript.SetStats(stats.speed, stats.maxHealth, stats.maxMana, stats.xpLevel, stats.maxLevelXP, stats.xp);
    }
}
