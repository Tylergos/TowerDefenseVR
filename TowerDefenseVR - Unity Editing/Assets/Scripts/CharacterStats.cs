using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public float speed = 1;
    public int maxHealth = 10;
    public int maxLevelXP
    {
        get
        {
            return xpLevel * 5;
        }
    }
    public int maxMana = 10;
    public int xpLevel = 5;
    public int xp = 5;
}
