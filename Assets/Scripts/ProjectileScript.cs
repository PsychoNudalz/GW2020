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
    [Header("Effect")]
    public ParticleSystem startPS;
    public ParticleSystem endPS;

    [Header("Seeker")]
    public bool isSeeker = false;
    public float seekerforce;
    public string targetTag = "Player";
    [Header("Target")]
    [SerializeField] GameObject shooter;
    [SerializeField] GameObject target;


    [Header("Extra")]
    public float spin;
    public string[] tagList = { "Enemy", "Object", "Obstacle", "MovableObject" };


    public void Awake()
    {
        if (isSeeker)
        {
            //target = GameObject.FindGameObjectWithTag(targetTag).transform;
            seekTarget();
            shoot();
        }
    }
    public void shoot()
    {
        ParticleSystem tempPS = Instantiate(startPS, transform.position, transform.rotation);
        tempPS.Play();
        Destroy(tempPS,tempPS.main.duration);
        //startPS.Play();
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
        if (target == null)
        {
            return;
        }
        transform.rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(target.transform.position-transform.position, Vector3.up),Vector3.forward);
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
            ParticleSystem tempPS =  Instantiate(endPS,transform.position,transform.rotation);
            tempPS.Play();
            Destroy(tempPS, tempPS.main.duration);

            Destroy(gameObject);
            EnemyScript e1;
            PlayerStates e2;
            DestructableScript e3;
            if (collision.TryGetComponent<EnemyScript>(out e1))
            {
                e1.takeDamage(damage);

            }
            if (collision.TryGetComponent<PlayerStates>(out e2))
            {
                e2.takeDamage(damage);

            }
            if (collision.TryGetComponent<DestructableScript>(out e3))
            {
                e3.takeDamage(damage);
            }
        }
    }

    public void setTarget(GameObject t)
    {
        target = t;
    }
    public void setShooter(GameObject t)
    {
        shooter = t;
    }
}
