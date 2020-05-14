using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Agent")
        {
            other.GetComponent<EnemyNavigation>().attackMode = true;
            other.GetComponent<EnemyNavigation>().attackObject = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Agent")
        {
            other.GetComponent<EnemyNavigation>().attackMode = false;
        }
    }
}
