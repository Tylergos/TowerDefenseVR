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

    private void OnTriggerStay(Collider _other)
    {
        if (a != null)
        {
            if (a.GetBool("Attack") && isPlayer)
            {
                if (_other.gameObject.tag == "Agent" && Time.time > nextHit)
                {
                    nextHit = Time.time + hitDelay;
                    enemy = _other.GetComponent<EnemyNavigation>();
                    if (!enemy.stun)
                    {
                        enemy.stun = true;
                        enemy.stunTime = Time.time + 1;

                        _other.attachedRigidbody.AddRelativeForce(Vector3.back * knockback);
                    }
                    if (enemy.invincibleTime <= Time.time)
                    {
                        enemy.health--;
                        enemy.invincibleTime = Time.time + 0.3f;
                    }
                }
            }
            if (a.GetBool("Attack") && !isPlayer)
            {
                if (_other.gameObject.tag == "Player")
                {
                    try
                    {
                        player = _other.GetComponent<CharacterScript>();
                        if (player.GetITime() <= Time.time)
                        {
                            player.ReduceHealth(1);
                            player.AddITime(0.666f);
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
