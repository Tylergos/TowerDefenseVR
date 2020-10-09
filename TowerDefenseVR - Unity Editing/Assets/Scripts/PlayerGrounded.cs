using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    CharacterScript p;

    private void Start()
    {
        p = this.GetComponentInParent<CharacterScript>();
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Ground")
        {
            p.ground = false;
        }
    }

    private void OnTriggerStay(Collider _other)
    {
        if (_other.tag == "Ground")
        {
            p.ground = true;
        }
    }
}
