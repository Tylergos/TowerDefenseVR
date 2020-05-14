using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrounded : MonoBehaviour
{
    EnemyNavigation e;

    // Start is called before the first frame update
    void Start()
    {
        e = this.gameObject.GetComponentInParent<EnemyNavigation>();
    }
    
    private void  OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Ground")
        {
            e.grounded = true;
            this.GetComponentInParent<EnemyNavigation>().NextLocation(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Ground")
        {
            e.grounded = false;
        }
    }
}
