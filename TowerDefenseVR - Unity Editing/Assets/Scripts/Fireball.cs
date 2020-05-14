using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    private float startTime;
    private float lifeTime;

    private void Start()
    {
        startTime = Time.time;
        lifeTime = 10;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.direction * Time.deltaTime * speed;

        if (Time.time >= startTime + lifeTime)
        {
            //Debug.Log("Destroyed after lifetime");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Agent")
        {
            Destroy(this.gameObject);
            EnemyNavigation enemy = other.GetComponent<EnemyNavigation>();
            enemy.health--;
            //Debug.Log("Hit Enemy");
        }
        if (other.tag == "Ground" || other.tag == "Wall")
        {
            Destroy(this.gameObject);
            //Debug.Log("Hit Ground or Wall");
        }
    }

    public void GetDirectionSpeed(Vector3 e, float s)
    {
        direction =Vector3.Normalize( e - this.transform.position);
        speed = s;
    }
}
