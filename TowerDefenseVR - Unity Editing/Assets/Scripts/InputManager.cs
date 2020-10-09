using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameObject player;
    CharacterScript playerScript;
    GameObject weapon;
    VRGun gunScript;
    OVRCameraRig cameraRig;
    Vector3 diff;
    Vector3 orig;
    //bool isPlacing;

    private void Start()
    {
        cameraRig = player.GetComponentInChildren<OVRCameraRig>();
        //isPlacing = false;
    }

    public void SetPlayer(GameObject _player)
    {
        this.player = _player;
        playerScript = _player.GetComponent<CharacterScript>();
    }

    protected void Update()
    {
        HeadRotation();
        playerScript.VRPlayerDirection(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        playerScript.Move();
        if (!(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && playerScript.IsBuildMode() && playerScript.isPlacing))
            playerScript.VRTurning(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
        if (OVRInput.Get(OVRInput.Button.One))
            playerScript.Jump();
        if (OVRInput.GetDown(OVRInput.Button.Three))
            playerScript.Build();

        if (playerScript.IsBuildMode())
        {
            playerScript.SelectTowerVR(OVRInput.GetDown(OVRInput.Button.Two));
            playerScript.CurSightTowerVR(cameraRig.rightControllerAnchor.position, cameraRig.rightControllerAnchor.transform.forward);
            playerScript.SpawnTower(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger), OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger), OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger), OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x);
        }
        else
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                weapon = playerScript.SelectWeaponVR(cameraRig);
                if (weapon.CompareTag("Gun"))
                {
                    gunScript = weapon.GetComponent<VRGun>();
                }
            }

            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && playerScript.GetCurWeapon().CompareTag("Gun"))
            {
                if (gunScript == null)
                {
                    gunScript = weapon.GetComponent<VRGun>();
                }
                gunScript.Fire();
            }

            if (weapon == null)
            {
                weapon = playerScript.GetCurWeapon();
                if (weapon == null)
                {
                    weapon = playerScript.SelectWeaponVR(cameraRig);
                }
            }
        }
    }

    private void HeadRotation()
    {
        //difference between camera and player rotation is calculated
        //diff is added to player parent and subtracted from camera
        diff = new Vector3(0, cameraRig.centerEyeAnchor.transform.eulerAngles.y - player.transform.eulerAngles.y, 0);
        player.transform.eulerAngles += diff;
        cameraRig.trackingSpace.eulerAngles -= diff;
    }
}
