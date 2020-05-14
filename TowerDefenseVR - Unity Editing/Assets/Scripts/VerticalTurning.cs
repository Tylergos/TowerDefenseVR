using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalTurning : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private CharacterScript playerScript;
    private float verticalSpeed = 2;
    float bb;
    Vector3 euler;

    private void Start()
    {
        playerScript = player.GetComponent<CharacterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Input.GetMouseButton(0) && playerScript.IsBuildMode() && playerScript.isPlacing))
        {
            euler.x += verticalSpeed * -Input.GetAxis("Mouse Y");
            if (euler.x < -100)
            {
                euler.x = -100;
            }
            if (euler.x > 35)
            {
                euler.x = 35;
            }
            this.transform.localEulerAngles = euler;
        }
    }
}
