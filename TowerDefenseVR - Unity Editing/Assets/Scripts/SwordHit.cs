using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [SerializeField]
    private float knockback;

    private Animator a;
    private EnemyNavigation enemy;
    private CharacterScript player;
    [SerializeField]
    private bool isPlayer;
    private float hitDelay = .1f;
    private float nextHit;
    

    private void Start()
    {
        a = this.GetComponentInParent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (a != null)
        {
            if (a.GetBool("Attack") && isPlayer)
            {
                if (other.gameObject.tag == "Agent" && Time.time > nextHit)
                {
                    nextHit = Time.time + hitDelay;
                    enemy = other.GetComponent<EnemyNavigation>();
                    if (!enemy.stun)
                    {
                        enemy.stun = true;
                        enemy.stunTime = Time.time + 1;

                        other.attachedRigidbody.AddRelativeForce(Vector3.back * knockback);
                    }
                    if (enemy.invincibleFrames <= 0)
                    {
                        enemy.health--;
                        enemy.invincibleFrames = 10;
                    }
                }
            }
            if (a.GetBool("Attack") && !isPlayer)
            {
                if (other.gameObject.tag == "Player")
                {
                    try
                    {
                        player = other.GetComponent<CharacterScript>();
                        if (player.GetIFrames() <= 0)
                        {
                            player.ReduceHealth(1);
                            player.SetIFrames(30);
                        }
                    } catch{ };
                }
            }
        }
    }

    private void Update()
    {
        if (a == null)
        {
            a = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Animator>();
        }
    }

}
