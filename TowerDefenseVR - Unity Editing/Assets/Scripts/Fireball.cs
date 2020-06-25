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
            Destroy(this.transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Agent")
        {
            Destroy(this.transform.parent.gameObject);
            EnemyNavigation enemy = other.GetComponent<EnemyNavigation>();
            enemy.health--;
        }
        if (other.tag == "Ground" || other.tag == "Wall")
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    public void SetDirectionSpeed(Vector3 e, float s)
    {
        direction = Vector3.Normalize( e - this.transform.position);
        speed = s;
    }
}
