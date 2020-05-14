using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private GameObject nextTeleporter;

    private CharacterScript c;
    private EnemyNavigation e;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            try
            {
                c = other.gameObject.GetComponent<CharacterScript>();
                c.teleporter = this;
                c.onTeleporter = true;
                c.teleporterCount = true;
            }
            catch { };
        }
        else if (other.gameObject.tag == "Agent")
        {
            e = other.gameObject.GetComponent<EnemyNavigation>();
            e.teleporter = this;
            e.onTeleporter = true;
            e.teleporterCount = true;
        }
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
                c.onTeleporter = false;
                if (c.waitForExit > 0)
                    c.waitForExit--;
            }
            catch { };
        }
        else if (other.gameObject.tag.Equals("Agent"))
        {
            e = other.gameObject.GetComponent<EnemyNavigation>();
            e.onTeleporter = false;
            if (e.waitForExit > 0)
                e.waitForExit--;
        }
    }
}
