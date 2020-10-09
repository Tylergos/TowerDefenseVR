using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Agent")
        {
            _other.GetComponent<EnemyNavigation>().attackMode = true;
            _other.GetComponent<EnemyNavigation>().attackObject = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Agent")
        {
            _other.GetComponent<EnemyNavigation>().attackMode = false;
        }
    }
}
