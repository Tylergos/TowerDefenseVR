﻿using System.Collections;
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

    private void Start()
    {
        if (endPoint)
        {
            //sets end point on AI pathfinding grid
            GameObject.FindGameObjectWithTag("AIGrid").GetComponent<Grid>().SetEndNode(this.transform.position);
        }
    }
}
