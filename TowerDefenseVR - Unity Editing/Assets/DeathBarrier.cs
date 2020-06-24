using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Agent")
        {
            other.GetComponent<EnemyNavigation>().health = 0;
        }
        if (other.tag == "Player")
        {
            other.GetComponent<CharacterScript>().ReduceHealth(int.MaxValue);
        }
    }
}
