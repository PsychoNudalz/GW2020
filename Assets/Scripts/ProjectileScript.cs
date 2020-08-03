using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [Header("States")]
    public Rigidbody2D rb;
    public float projectileforce;
    public float maxVelocity;
    public float damage;
    public float timeToLive = 1f;
    [Header("Seeker")]
    public bool isSeeker = false;
    public float seekerforce;
    public string targetTag = "Player";
    [SerializeField] Transform target;
    [Header("Extra")]
    public float spin;
    public string[] tagList = { "Enemy", "Object", "Obstacle" };


    public void Awake()
    {
        if (isSeeker)
        {
            target = GameObject.FindGameObjectWithTag(targetTag).transform;
            seekTarget();
            shoot();
        }
    }
    public void shoot()
    {
        rb.AddForce(transform.up * projectileforce * rb.mass);
        //print(transform.forward * projectileforce * rb.mass);
        if (spin != 0)
        {
            rb.AddTorque(spin);
        }


    }

    private void Update()
    {
        if (isSeeker)
        {
            seekTarget();
        }
        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            timeToLive -= Time.deltaTime;
        }
    }

    private void seekTarget()
    {
        transform.rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(target.position-transform.position, Vector3.up),Vector3.forward);
        rb.AddForce(transform.up * seekerforce*Time.deltaTime * rb.mass);
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity / 0.9f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tagList.Contains(collision.tag))
        {
            Destroy(gameObject);
            EnemyScript e1;
            PlayerStates e2;
            if (collision.TryGetComponent<EnemyScript>(out e1))
            {
                e1.takeDamage(damage);

            }
            if (collision.TryGetComponent<PlayerStates>(out e2))
            {
                e2.takeDamage(damage);

            }
        }
    }
}
