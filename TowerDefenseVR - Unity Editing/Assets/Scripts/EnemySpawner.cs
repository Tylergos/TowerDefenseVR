﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject spawn;
    private int enemies;
    private int spawntime;
    private float statMod;
    [SerializeField]
    private GameObject[] enemyTypes;
    private GameObject[] currentEnemyTypes;
    private float nextSpawn;
    [SerializeField]
    private float spawnDelay;
    private GameObject parent;
    private bool roundOver;
    private float nextRoundTime;
    private float roundDelay;
    private GameObject player;
    private GameObject newEnemy;

    // Start is called before the first frame update
    void Start()
    {
        roundOver = false;
        this.gameObject.name = "Enemy Spawner";
        spawn = GameObject.FindGameObjectWithTag("Spawnpoint");
        parent = GameObject.FindGameObjectWithTag("Entities");
        spawnDelay = 1;
        roundDelay = 2;
        CalculateNumEnemies();
        CalculateEnemyStatMod();
        SelectEnemyTypes();
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    private void CalculateNumEnemies()
    {
        enemies = Bag.bag.GetLevel() * 1;
        Bag.bag.SetEnemiesRemaining(enemies);
    }

    private void CalculateEnemyStatMod()
    {
        statMod = (1 + (Bag.bag.GetLevel() * 0.05f));
    }

    private void SelectEnemyTypes()
    {
        currentEnemyTypes = new GameObject[enemies];
        for (int i = 0; i < enemies; i++)
        {
            currentEnemyTypes[i] = enemyTypes[Random.Range(0, enemyTypes.Length)];
        }
    }

    private void SpawnEnemies()
    {
        if (enemies > 0)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnDelay;
                newEnemy = Instantiate(currentEnemyTypes[enemies - 1], spawn.transform.position, spawn.transform.rotation, parent.transform);
                newEnemy.GetComponent<EnemyNavigation>().setPlayer(player);
                enemies--;
            }
        }
    }

    private void IsAllDead()
    {
        if (Bag.bag.GetEnemiesRemaining() <= 0)
        {
            roundOver = true;
            nextRoundTime = Time.time + roundDelay;
            Bag.bag.SetLevel(Bag.bag.GetLevel() + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!roundOver)
        {
            IsAllDead();
            SpawnEnemies();
        }
        else
        {
            if (Time.time > nextRoundTime)
            {
                GameObject newSpawner = Instantiate(this.gameObject);
                newSpawner.GetComponent<EnemySpawner>().SetPlayer(player);
                Destroy(this.gameObject);
            }
        }
    }
}