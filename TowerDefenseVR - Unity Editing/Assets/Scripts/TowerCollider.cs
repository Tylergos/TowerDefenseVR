using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCollider : MonoBehaviour
{
    private List<GameObject> enemies;

    private void Start()
    {
        enemies = new List<GameObject>();
    }

    public List<GameObject> GetEnemiesInView()
    {
        return enemies;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Agent")
        {
            try
            {
                enemies.Add(_other.gameObject);
            }
            catch {
                //triggered when enemy in tower collider and player
                //looks at enemy while attempting to spawn tower
                //Debug.Log("False on trigger enter caused by: " + other);
            };
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Agent" && enemies.Contains(_other.gameObject))
        {
            enemies.Remove(_other.gameObject);
        }
    }
}
