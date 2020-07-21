using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private GameObject nextTeleporter;
    [SerializeField]
    private int linkNum;

    [SerializeField]
    private int gridRadius;

    private CharacterScript c;
    private EnemyNavigation e;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("AIGrid").GetComponent<Grid>().AddTeleporterNodes(this.gameObject, gridRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            try
            {
                c = other.gameObject.GetComponent<CharacterScript>();
                c.teleporter = this;
                c.teleporterCount = true;
            }
            catch { };
        }
        else if (other.gameObject.tag == "Agent")
        {
            e = other.gameObject.GetComponent<EnemyNavigation>();
            e.teleporter = this;
            e.teleporterCount = true;
        }
    }

    public GameObject GetNextTeleporter()
    {
        return nextTeleporter;
    }

    public int GetLinkNum()
    {
        return linkNum;
    }

    public void Teleport(GameObject g)
    {
        g.transform.position = nextTeleporter.transform.position + new Vector3(0, g.GetComponent<Collider>().bounds.extents.y);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            try
            {
                c = other.gameObject.GetComponent<CharacterScript>();
                c.teleporter = null;
            }
            catch { };
        }
        else if (other.gameObject.tag.Equals("Agent"))
        {
            e = other.gameObject.GetComponent<EnemyNavigation>();
            e.teleporter = null;
        }
    }
}
