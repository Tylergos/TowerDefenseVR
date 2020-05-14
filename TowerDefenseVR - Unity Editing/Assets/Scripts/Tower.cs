using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tower : MonoBehaviour
{
    public int health;
    LayerMask groundWallsEnemies;
    private GameObject[] visibleEnemies;
    private Ray ray;
    private float closest;
    private int highestPoint;
    private EnemyNavigation e;
    private RaycastHit hit;
    private TowerCollider viewCollider;
    private GameObject target;
    private bool start;
    private NavMeshObstacle o;
    private Vector3 shotLocation;
    private float distanceTime;

    private float bulletSpeed;
    private float shotTimer;
    private float nextShot;
    

    [SerializeField]
    private GameObject fireball;

    // Start is called before the first frame update
    void Start()
    {
        shotTimer = 2;
        bulletSpeed = 15;
        viewCollider = this.gameObject.GetComponentInChildren<TowerCollider>();
        groundWallsEnemies = LayerMask.GetMask("Enemy") | LayerMask.GetMask("GroundAndWalls");
        o = this.GetComponent<NavMeshObstacle>();
        o.enabled = false;
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        Sight();
        AimAhead();
    }

    private void AimAhead()
    {
        if (target != null)
        {
            e = target.GetComponent<EnemyNavigation>();

            distanceTime = Vector3.Distance(this.transform.position, e.transform.position) / bulletSpeed;
            shotLocation = e.transform.position + (e.transform.forward * e.GetSpeed() * distanceTime);
            Shoot(shotLocation);
        }
    }


    private void Shoot(Vector3 e)
    {
        if (Time.time > nextShot)
        {
            GameObject g = Instantiate(fireball, this.transform.position, this.transform.rotation);
            g.GetComponent<Fireball>().GetDirectionSpeed(e, bulletSpeed);
            nextShot = Time.time + shotTimer;
        }
    }
    
    private void Sight()
    {
        //checks if tower is placed
        if (this.gameObject.layer == 12)
        {
            target = null;
            visibleEnemies = viewCollider.GetEnemiesInView();
            closest = float.PositiveInfinity;
            highestPoint = -1;
            for (int i = 0; i < viewCollider.GetEnemiesInView().Length; i++)
            {
                if (visibleEnemies[i] != null)
                {
                    ray = new Ray(this.transform.position, visibleEnemies[i].transform.position - this.transform.position);
                    Physics.Raycast(ray, out hit, float.MaxValue, groundWallsEnemies);
                    if (hit.transform.tag == "Agent")
                    {
                        e = visibleEnemies[i].GetComponent<EnemyNavigation>();
                        if (e.GetDistance() < closest && e.GetCurPoint() == highestPoint)
                        {
                            target = visibleEnemies[i];
                            closest = e.GetDistance();
                        }
                        else if (highestPoint < e.GetCurPoint())
                        {
                            target = visibleEnemies[i];
                            closest = e.GetDistance();
                            highestPoint = e.GetCurPoint();
                        }
                    }
                }
            }
        }
    }
}