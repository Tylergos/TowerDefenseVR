using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    float velocityY;
    float angVelocityY;

    private Vector3 direction;
    private Rigidbody rb;

    [HideInInspector]
    public bool ground;

    private float jumpSleep;
    private Collider col;

    [SerializeField]
    private GameObject[] towers;
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private GameObject gunPrefab;
    [SerializeField]
    private GameObject playerCamera;
    private GameObject healthBar;
    private GameObject manaBar;
    private GameObject xpBar;

    private Animator gunAnimator;
    private Animator swordAnimator;

    private bool buildMode;

    private int curWeaponNum;
    private GameObject curWeapon;

    private int totalTowerNum;
    private int curTowerNum;
    private GameObject curTower;
    private float horizontalSpeed = 2;
    private Collider unplacedCollider;

    private LayerMask unplacedTowerMask;
    private LayerMask placedTowerMask;
    private LayerMask enemyMask;

    private Renderer unplacedTowerShader;

    private EnemyNavigation enemy;

    private bool canPlace;

    [HideInInspector]
    public bool shotFired;

    private bool rotationMode;

    public Dictionary<int, GameObject> placedTowers = new Dictionary<int, GameObject>();
    GameObject LastTower;

    private float teleporterTime;
    [HideInInspector]
    public bool teleporterCount = false;
    [HideInInspector]
    public Teleporter teleporter;

    [SerializeField]
    private int maxHealth;
    private int health;
    [SerializeField]
    private int maxLevelXP;
    private int xp;
    [SerializeField]
    private int maxMana;
    private int mana;

    private float invincibleTime;

    private bool isVR;

    public bool isPlacing;

    private Grid aiGrid;

    // Use this for initialization
    protected void Start()
    {
        teleporterTime = Time.time;
        jumpSleep = Time.time;
        shotFired = false;
        canPlace = false;
        totalTowerNum = 0;
        unplacedTowerMask = LayerMask.GetMask("UnplacedTower");
        unplacedTowerMask |= Physics.IgnoreRaycastLayer;
        unplacedTowerMask = ~unplacedTowerMask;
        enemyMask = LayerMask.GetMask("Enemy");
        placedTowerMask = LayerMask.GetMask("PlacedTower");
        curWeaponNum = 0;
        curTowerNum = 0;
        buildMode = false;
        if (!isVR)
            SelectWeaponDesktop();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent<Collider>();
        rotationMode = false;
        invincibleTime = Time.time;
        xp = 0;
        xpBar = GameObject.FindGameObjectWithTag("UIXP");
        healthBar = GameObject.FindGameObjectWithTag("UIHealth");
        manaBar = GameObject.FindGameObjectWithTag("UIMana");
        health = maxHealth;
        isPlacing = false;

        //Updates UI
        IncreaseHealth(0);
        IncreaseXP(0);
        IncreaseMana(0);


        aiGrid = GameObject.FindGameObjectWithTag("AIGrid").GetComponent<Grid>();
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (teleporterCount)
        {
            if (teleporterTime <= Time.time && teleporter != null)
            {
                teleporterCount = false;
                teleporter.Teleport(this.gameObject);
                teleporterTime = Time.time + 3f;
            }
        }

        //resets velocity to prevent sliding
        velocityY = rb.velocity.y;
        angVelocityY = rb.angularVelocity.y;
        rb.velocity.Set(0, velocityY, 0);
        rb.angularVelocity.Set(0, angVelocityY, 0);

        //movements
        if (!isVR)
        {
            PlayerDirection();
            Move();
            if (!(Input.GetMouseButton(0) && buildMode && isPlacing))
                Turning();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        //building and attacking
        if (Input.GetKeyDown(KeyCode.B))
            Build();
        if (buildMode && !isVR)
        {
            SelectTowerDesktop();
            CurSight();
            SpawnTower(Input.GetMouseButtonDown(0), Input.GetMouseButton(0), Input.GetMouseButtonUp(0), Input.GetAxis("Mouse X"));
        }
        else if (!buildMode && !isVR)
        {
            SelectWeaponDesktop();
            AttackDesktop();
        }

        //sets player position node in AI grid
        aiGrid.SetPlayerNode(transform.position);
    }

    public bool IsBuildMode()
    {
        return buildMode;
    }

    public void AddITime(float seconds)
    {
        if (Time.time + seconds > invincibleTime)
        {
            invincibleTime = Time.time + seconds;
        }
    }

    public float GetITime()
    {
        return invincibleTime;
    }

    public GameObject GetCurWeapon()
    {
        return curWeapon;
    }

    public void IsVR(bool isVR)
    {
        this.isVR = isVR;
    }

    public void CurSightTowerVR(Vector3 origin, Vector3 dir)
    {
        RaycastHit hit;
        canPlace = false;

        if (Physics.Raycast(origin, dir, out hit, 8, unplacedTowerMask | placedTowerMask) && curTower != null && !rotationMode)
        {
            if (!Physics.CheckSphere(hit.point + new Vector3(0, .5f, 0), .45f, unplacedTowerMask) && !Physics.CheckSphere(hit.point + new Vector3(0, .5f, 0), .55f, placedTowerMask | enemyMask))
            {
                curTower.transform.position = hit.point + new Vector3(0, .5f, 0);
                canPlace = true;
            }
            else
            {
                Destroy(curTower);
            }
        }
        else
        {
            Destroy(curTower);
        }
    }

    private void CurSight()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        canPlace = false;
        //determines whether tower can or cant be place in location
        if (buildMode)
        {
            if (Physics.Raycast(ray, out hit, 8, unplacedTowerMask | placedTowerMask) && curTower != null && !rotationMode)
            {
                if (!Physics.CheckSphere(hit.point + new Vector3(0, .5f, 0), .45f, unplacedTowerMask) && !Physics.CheckSphere(hit.point + new Vector3(0, .5f, 0), .55f, placedTowerMask | enemyMask))
                {
                    curTower.transform.position = hit.point + new Vector3(0, .5f, 0);
                    canPlace = true;
                }
                else
                {
                    Destroy(curTower);
                }
            }
            else
            {
                Destroy(curTower);
            }
        }

        //determines if shot from main gun hits
        else if (!buildMode)
        {
            if (Physics.Raycast(ray, out hit, 50, unplacedTowerMask))
            {
                if (hit.collider.CompareTag("Agent"))
                {
                    enemy = hit.transform.gameObject.GetComponent<EnemyNavigation>();
                    if (!enemy.stun)
                    {
                        enemy.stun = true;
                        enemy.stunTime = Time.time + 1;

                        hit.rigidbody.AddRelativeForce(Vector3.back * 200);
                    }
                    if (enemy.invincibleTime <= Time.time)
                    {
                        enemy.health--;
                        enemy.invincibleTime = Time.time + 0.3f;
                    }
                }
            }
        }
    }

    //sets buildmode as active or inactive
    public void Build()
    {
        if (buildMode)
        {
            buildMode = false;
            Destroy(curTower);
        }
        else
        {
            buildMode = true;
            Destroy(curWeapon);
            if (isVR)
            {
                SelectTowerVR(false);
            }
        }
    }

    public GameObject SelectWeaponVR(OVRCameraRig rig)
    {
        if (curWeapon == null)
        {
            if (curWeaponNum == 0)
            {
                curWeapon = Instantiate(gunPrefab, rig.rightControllerAnchor);
                gunAnimator = curWeapon.GetComponent<Animator>();
            }
            else if (curWeaponNum == 1)
            {
                curWeapon = Instantiate(swordPrefab, rig.rightControllerAnchor);
                swordAnimator = curWeapon.GetComponent<Animator>();
            }
        }
        else if (!(curWeaponNum == 0))
        {
            curWeaponNum = 0;
            Destroy(curWeapon);
            curWeapon = Instantiate(gunPrefab, rig.rightControllerAnchor);
            gunAnimator = curWeapon.GetComponent<Animator>();
        }
        else if (!(curWeaponNum == 1) && gunAnimator.GetBool("Attack") == false)
        {
            curWeaponNum = 1;
            Destroy(curWeapon);
            curWeapon = Instantiate(swordPrefab, rig.rightControllerAnchor);
            swordAnimator = curWeapon.GetComponent<Animator>();
        }
        return curWeapon;
    }

    //select weapons
    private void SelectWeaponDesktop()
    {
        if (curWeapon == null)
        {
            if (curWeaponNum == 0)
            {
                curWeapon = Instantiate(gunPrefab, this.transform);
                gunAnimator = curWeapon.GetComponent<Animator>();
            }
            else if (curWeaponNum == 1)
            {
                curWeapon = Instantiate(swordPrefab, this.transform);
                swordAnimator = curWeapon.GetComponent<Animator>();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !(curWeaponNum == 0) && swordAnimator.GetBool("Attack") == false)
        {
            curWeaponNum = 0;
            Destroy(curWeapon);
            curWeapon = Instantiate(gunPrefab, this.transform);
            gunAnimator = curWeapon.GetComponent<Animator>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !(curWeaponNum == 1) && gunAnimator.GetBool("Attack") == false)
        {
            curWeaponNum = 1;
            Destroy(curWeapon);
            curWeapon = Instantiate(swordPrefab, this.transform);
            swordAnimator = curWeapon.GetComponent<Animator>();
        }
    }

    //attacks with current weapon
    private void AttackDesktop()
    {
        if (Input.GetMouseButton(0))
        {
            if (curWeaponNum == 0)
            {
                gunAnimator.SetBool("Attack", true);
                if (!shotFired)
                {
                    CurSight();
                    shotFired = true;
                }
            }
            else if (curWeaponNum == 1 && !isVR)
            {
                swordAnimator.SetBool("Attack", true);
            }
        }
    }

    public void SelectTowerVR(bool input)
    {
        if (curTower == null)
        {
            curTower = Instantiate(towers[curTowerNum], this.transform);
            unplacedTowerShader = curTower.GetComponent<Renderer>();
            if (curTowerNum == 0)
            {
                unplacedTowerShader.material.color = new Color(.5f, 1, .5f);
            }
            else if (curTowerNum == 1)
            {
                unplacedTowerShader.material.color = new Color(1, 1, .5f);
            }
            else if (curTowerNum == 2)
            {
                unplacedTowerShader.material.color = new Color(.5f, .5f, 1);
            }
            else if (curTowerNum == 3)
            {
                unplacedTowerShader.material.color = new Color(1, .5f, .5f);
            }
            unplacedCollider = curTower.GetComponent<Collider>();
            unplacedCollider.enabled = false;
        }
        else if (input)
        {
            Destroy(curTower);
            curTowerNum++;
            if (curTowerNum >= towers.Length)
            {
                curTowerNum = 0;
            }
            curTower = Instantiate(towers[curTowerNum], this.transform);
            unplacedTowerShader = curTower.GetComponent<Renderer>();
            unplacedCollider = curTower.GetComponent<Collider>();
            if (curTowerNum == 0)
            {
                unplacedTowerShader.material.color = new Color(.5f, 1, .5f);
            }
            else if (curTowerNum == 1)
            {
                unplacedTowerShader.material.color = new Color(1, 1, .5f);
            }
            else if (curTowerNum == 2)
            {
                unplacedTowerShader.material.color = new Color(.5f, .5f, 1);
            }
            else if (curTowerNum == 3)
            {
                unplacedTowerShader.material.color = new Color(1, .5f, .5f);
            }
            unplacedCollider.enabled = false;
        }
    }

    //select towers
    private void SelectTowerDesktop()
    {
        if (curTower == null)
        {
            curTower = Instantiate(towers[curTowerNum], this.transform);
            unplacedTowerShader = curTower.GetComponent<Renderer>();
            if (curTowerNum == 0)
            {
                unplacedTowerShader.material.color = new Color(.5f, 1, .5f);
            }
            else if (curTowerNum == 1)
            {
                unplacedTowerShader.material.color = new Color(1, 1, .5f);
            }
            else if (curTowerNum == 2)
            {
                unplacedTowerShader.material.color = new Color(.5f, .5f, 1);
            }
            else if (curTowerNum == 3)
            {
                unplacedTowerShader.material.color = new Color(1, .5f, .5f);
            }
            unplacedCollider = curTower.GetComponent<Collider>();
            unplacedCollider.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && !(curTowerNum == 0))
        {
            Destroy(curTower);
            curTowerNum = 0;
            curTower = Instantiate(towers[curTowerNum], this.transform);
            unplacedTowerShader = curTower.GetComponent<Renderer>();
            unplacedTowerShader.material.color = new Color(.5f, 1, .5f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !(curTowerNum == 1))
        {
            Destroy(curTower);
            curTowerNum = 1;
            curTower = Instantiate(towers[curTowerNum], this.transform);
            unplacedTowerShader = curTower.GetComponent<Renderer>();
            unplacedTowerShader.material.color = new Color(1, 1, .5f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !(curTowerNum == 2))
        {
            Destroy(curTower);
            curTowerNum = 2;
            curTower = Instantiate(towers[curTowerNum], this.transform);
            unplacedTowerShader = curTower.GetComponent<Renderer>();
            unplacedTowerShader.material.color = new Color(.5f, .5f, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && !(curTowerNum == 3))
        {
            Destroy(curTower);
            curTowerNum = 3;
            curTower = Instantiate(towers[curTowerNum], this.transform);
            unplacedTowerShader = curTower.GetComponent<Renderer>();
            unplacedTowerShader.material.color = new Color(1, .5f, .5f);
        }
        unplacedCollider = curTower.GetComponent<Collider>();
        unplacedCollider.enabled = false;
    }

    //Spawns selected tower
    public void SpawnTower(bool buttonDown, bool buttonCurDown, bool buttonUp, float rotation)
    {

        if (buttonDown && canPlace)
        {
            LastTower = Instantiate(towers[curTowerNum], curTower.transform.position, curTower.transform.rotation);
            placedTowers.Add(LastTower.GetInstanceID(), LastTower);
            totalTowerNum++;
            rotationMode = true;
        }
        if (buttonCurDown && rotationMode)
        {
            try
            {
                LastTower.transform.Rotate(0, horizontalSpeed * rotation, 0);
            }
            catch { };
        }
        if (buttonUp)
        {
            try
            {
                LastTower.GetComponent<Tower>().TowerPlaced();
                rotationMode = false;
            }
            catch { };
        }
        isPlacing = rotationMode;
    }

    public void ReduceHealth(int damage)
    {
        if (invincibleTime <= Time.time)
        {
            health -= damage;
            if (health < 0)
            {
                //Debug.Log("Player died, current health:" + health);
            }
            healthBar.GetComponent<HealthBar>().ChangeHealth(health, maxHealth);
        }
    }

    public void IncreaseHealth(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.GetComponent<HealthBar>().ChangeHealth(health, maxHealth);
    }

    public void IncreaseMana(int mana)
    {
        this.mana += mana;
        if (this.mana > maxMana)
        {
            this.mana = maxMana;
        }
        manaBar.GetComponent<ManaBar>().ChangeMana(this.mana, maxMana);
    }

    public void DecreaseMana(int mana)
    {
        this.mana -= mana;
        if (this.mana < 0)
        {
            this.mana = 0;
        }
        manaBar.GetComponent<ManaBar>().ChangeMana(this.mana, maxMana);
    }

    public void IncreaseXP(int gain)
    {
        xp += gain;
        if (xp >= maxLevelXP)
        {
            xp -= maxLevelXP;
        }
        xpBar.GetComponent<XPBar>().ChangeXP(xp, maxLevelXP);
    }

    //jumping
    public void Jump()
    {
        if (ground && rb.velocity.y < 0.1 && rb.velocity.y > -0.1 && jumpSleep <= Time.time)
        {
            jumpSleep = Time.time + 0.1f;
            rb.AddForce(0, 20000, 0);
        }
    }

    public void Move()
    {
        direction.Normalize();
        this.gameObject.transform.Translate(speed * direction.x * Time.deltaTime, 0, speed * direction.z * Time.deltaTime);
    }

    protected void Turning()
    {
        this.transform.Rotate(0, horizontalSpeed * Input.GetAxis("Mouse X"), 0);
    }

    public void VRTurning(Vector2 input)
    {
        this.transform.Rotate(0, horizontalSpeed * input.x, 0);
    }

    public void VRPlayerDirection(Vector2 input)
    {
        direction = Vector3.zero;

        direction += Vector3.forward * input.y;

        direction += Vector3.right * input.x;
    }

    protected void PlayerDirection()
    {
        direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
    }
}
