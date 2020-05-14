using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator a;
    GameObject g;
    CharacterScript p;

    private void Start()
    {
        a = this.gameObject.GetComponent<Animator>();
        g = this.gameObject;

        //If this object is gun, get the player script for shooting
        if (g.tag == "Gun")
        {
            p = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        }
    }

    private void DoThis()
    {
        //Used to reset attack after animation is done
        a.SetBool("Attack", false);
        if (g.tag == "Gun")
        {
            p.shotFired = false;
        }
    }
}
