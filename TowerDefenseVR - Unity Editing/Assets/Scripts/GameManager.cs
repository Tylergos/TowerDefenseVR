using System.Collections;
using System.Collections.Generic;
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
            player = Instantiate(playerPrefab, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.rotation);
            player.GetComponent<CharacterScript>().IsVR(isVR);
            enemySpawner.GetComponent<EnemySpawner>().SetPlayer(player);
        }
        else if (GameObject.FindGameObjectWithTag("Player") == null && isVR)
        {
            player = Instantiate(playerVRPrefab, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position, GameObject.FindGameObjectWithTag("PlayerSpawn").transform.rotation);
            player.GetComponent<CharacterScript>().IsVR(isVR);
            enemySpawner.GetComponent<EnemySpawner>().SetPlayer(player);
            GameObject manager = Instantiate(inputManager);
            manager.GetComponent<InputManager>().SetPlayer(player);
        }
    }
}
