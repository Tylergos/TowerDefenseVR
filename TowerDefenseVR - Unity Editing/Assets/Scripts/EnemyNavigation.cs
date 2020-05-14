using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEditor;

public class EnemyNavigation : MonoBehaviour
{
    private GameObject player;

    [HideInInspector]
    public bool stun;
    [HideInInspector]
    public float stunTime;
    [HideInInspector]
    public int invincibleFrames;
    
    public int health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private int experience;

    Rigidbody rb;

    [HideInInspector]
    public bool attackMode;
    [HideInInspector]
    public GameObject attackObject;

    private Collider col;
    LayerMask unplacedTowerMask;

    [HideInInspector]
    public int tCountdown = 60;
    [HideInInspector]
    public bool teleporterCount = false;
    [HideInInspector]
    public bool onTeleporter = false;
    [HideInInspector]
    public Teleporter teleporter;
    [HideInInspector]
    public int waitForExit;

    private GroundScript gs;
    private int pathChoice;
    private int curPoint;
    private Transform point;
    private Vector3 distanceV3;
    public bool grounded;
    private Quaternion lookRotation;

    private Animator meleeAnimator;


    // Start is called before the first frame update
    void Start()
    {
        grounded = false;
        waitForExit = 0;
        //creates mask for placing tower preview
        unplacedTowerMask = LayerMask.GetMask("UnplacedTower");
        unplacedTowerMask = ~unplacedTowerMask;

        col = this.GetComponent<Collider>();
        attackMode = false;
        invincibleFrames = 10;
        rb = this.GetComponent<Rigidbody>();
        stun = false;
        pathChoice = Random.Range(0, 2);
        meleeAnimator = this.GetComponentInChildren<Animator>();
    }

    public void setPlayer(GameObject player)
    {
        //sets playerscript for this object
        this.player = player;
    }

    public int GetCurPoint()
    {
        //returns current path point
        return curPoint;
    }

    public float GetDistance()
    {
        //returns the distance from this object to the next location
        return Vector3.Distance(this.gameObject.transform.position, point.position);
    }

    public float GetSpeed()
    {
        //returns the speed
        return speed;
    }

    public void NextLocation(Collider collider)
    {
        //sets the next location of this object to the path marker from the ground
        gs = collider.gameObject.GetComponent<GroundScript>();
        curPoint = gs.curPoint;
        point = gs.markers[pathChoice].gameObject.transform;
    }

    private void Movement()
    {
        //movement and rotation of this object
        distanceV3 = point.position - this.gameObject.transform.position;
        lookRotation = Quaternion.LookRotation(distanceV3.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        
        this.gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void Attacking(GameObject other)
    {
        //Used for attacking another object
        float distance = (this.transform.position - other.transform.position).magnitude;
        //When outside the range of attack move towards the object
        if (distance > attackRange)
        {
            distanceV3 = other.transform.position - this.gameObject.transform.position;
            distanceV3.y = 0;
            lookRotation = Quaternion.LookRotation(distanceV3.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            this.gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        //When inside the range attack
        else
        {
            distanceV3 = other.transform.position - this.gameObject.transform.position;
            distanceV3.y = 0;
            lookRotation = Quaternion.LookRotation(distanceV3.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            meleeAnimator.SetBool("Attack", true);
        }
    }

    public void ReachedEnd()
    {
        //When the end is reached update Bag and destroy this object
        Bag.bag.SetEnemiesRemaining(Bag.bag.GetEnemiesRemaining() - 1);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (grounded && !stun && !attackMode)
        {
            Movement();
        }
        else if (grounded && !stun && attackMode)
        {
            Attacking(attackObject);
        }
        
        if (invincibleFrames > 0)
        {
            invincibleFrames--;
        }

        if (stun && Time.time > stunTime)
        {
            stun = false;
        }

        if (health <= 0)
        {
            //Update Bag and player xp, and destroy this object after death
            Bag.bag.SetEnemiesRemaining(Bag.bag.GetEnemiesRemaining() - 1);
            player.GetComponent<CharacterScript>().IncreaseXP(1);
            Destroy(this.gameObject);
        }

        if (teleporterCount)
        {
            tCountdown--;
            //prevents teleport looping
            if (tCountdown <= 0 && onTeleporter && waitForExit == 0)
            {
                onTeleporter = false;
                teleporterCount = false;
                teleporter.Teleport(this.gameObject);
                tCountdown = 60;
                waitForExit = 2;
            }
            else if (tCountdown <= 0 && !onTeleporter)
            {
                onTeleporter = false;
                tCountdown = 60;
                teleporterCount = false;
            }
        }
    }
}
