using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAvoidance : MonoBehaviour
{/*
    private Collider parentC;
    private EnemyNavigation parentE;
    private EnemyNavigation otherE;
    private Vector3 otherVelocity;
    private Vector3 thisVelocity;
    private Vector3 temporaryPoint;
    private NavMeshHit hit;

    public GameObject spawnpoint;

    private void Start()
    {
        parentE = this.GetComponentInParent<EnemyNavigation>();
        parentC = this.GetComponentInParent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Agent")
        {
            otherE = other.gameObject.GetComponent<EnemyNavigation>();
            otherVelocity = otherE.agent.velocity;
            thisVelocity = parentE.agent.velocity;
            if (otherVelocity.magnitude - thisVelocity.magnitude < -0.1)
            {
                //check left
                temporaryPoint = new Vector3(other.transform.position.x + Vector3.Normalize(otherVelocity).z * (other.bounds.extents.x + (parentC.bounds.extents.x * 2) + 0.1f), other.transform.position.y, other.transform.position.z + Vector3.Normalize(otherVelocity).x * (other.bounds.extents.x + (parentC.bounds.extents.x * 2) + 0.1f));
                if (NavMesh.SamplePosition(temporaryPoint, out hit, .6f, NavMesh.AllAreas))
                {
                    parentE.agent.destination = temporaryPoint;
                    parentE.SetCurPoint(parentE.GetCurPoint() - 1);
                    
                }
                //checks right
                else
                {
                    temporaryPoint = new Vector3(other.transform.position.x + -Vector3.Normalize(otherVelocity).z * (other.bounds.extents.x + (parentC.bounds.extents.x * 2) + 0.1f), other.transform.position.y, other.transform.position.z + -Vector3.Normalize(otherVelocity).x * (other.bounds.extents.x + (parentC.bounds.extents.x * 2) + 0.1f));
                    if (NavMesh.SamplePosition(temporaryPoint, out hit, .6f, NavMesh.AllAreas))
                    {
                        parentE.agent.destination = temporaryPoint;
                        parentE.SetCurPoint(parentE.GetCurPoint() - 1);
                    }
                }
            }
        }
    }*/
}