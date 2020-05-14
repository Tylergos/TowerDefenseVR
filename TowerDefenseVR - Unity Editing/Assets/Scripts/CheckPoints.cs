using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField]
    private bool endPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (endPoint && other.tag == "Agent")
        {
            other.GetComponent<EnemyNavigation>().ReachedEnd();
        }
    }
}
