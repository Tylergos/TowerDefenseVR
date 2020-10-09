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

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.tag == "Agent")
        {
            Destroy(this.transform.parent.gameObject);
            EnemyNavigation enemy = _other.GetComponent<EnemyNavigation>();
            enemy.health--;
        }
        if (_other.tag == "Ground" || _other.tag == "Wall")
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    public void SetDirectionSpeed(Vector3 _e, float _s)
    {
        direction = Vector3.Normalize( _e - this.transform.position);
        speed = _s;
    }
}
