using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollider : MonoBehaviour
{

    private GameObject[] enemies = new GameObject[99];
    private int numEnemies;

    private void Start()
    {
        numEnemies = 0;
    }

    public int GetNumEnemies()
    {
        return numEnemies;
    }

    public GameObject[] GetEnemiesInView()
    {
        return enemies;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Agent")
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] == null)
                {
                    enemies[i] = other.gameObject;
                    numEnemies++;
                    break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Agent")
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] == other.gameObject)
                {
                    enemies[i] = null;
                    numEnemies--;
                    break;
                }
            }
        }
    }
}
