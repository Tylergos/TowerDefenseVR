using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGun : MonoBehaviour
{
    Animator a;
    private LayerMask unplacedTowerMask;
    private EnemyNavigation enemy;
    private float fireDelay = 0.8f;
    private float nextShot;

    // Start is called before the first frame update
    void Start()
    {
        a = this.gameObject.GetComponent<Animator>();

        unplacedTowerMask = LayerMask.GetMask("UnplacedTower");
        unplacedTowerMask = ~unplacedTowerMask;
        nextShot = Time.time + fireDelay;
    }

    public void Fire()
    {
        RaycastHit hit;

        if (Time.time >= nextShot && Physics.Raycast(this.transform.position, this.transform.forward, out hit, 50, unplacedTowerMask))
        {
            nextShot = Time.time + fireDelay;
            if (hit.collider.CompareTag("Agent"))
            {
                enemy = hit.transform.gameObject.GetComponent<EnemyNavigation>();
                if (!enemy.stun)
                {
                    enemy.stun = true;
                    enemy.stunTime = Time.time + 1;
                    hit.rigidbody.AddRelativeForce(Vector3.back * 200);
                }
                if (enemy.invincibleFrames <= 0)
                {
                    enemy.health--;
                    enemy.invincibleFrames = 10;
                }
            }
        }
    }
}
