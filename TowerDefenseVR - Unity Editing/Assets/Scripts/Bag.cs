using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour{

    private int level;
    public static Bag bag;
    private int enemiesRemaining;

    public int GetLevel()
    {
        return level;
    }
    public void SetLevel(int a)
    {
        level = a;
    }

    public int GetEnemiesRemaining()
    {
        return enemiesRemaining;
    }
    public void SetEnemiesRemaining(int _a)
    {
        enemiesRemaining = _a;
    }


    // Use this for initialization
    void Awake () {
        bag = this;
        level = 1;
        enemiesRemaining = 0;
    }
    
}
