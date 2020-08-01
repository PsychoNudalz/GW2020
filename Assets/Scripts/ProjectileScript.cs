using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public float projectileforce;
    public float damage;
    public float timeToLive = 1f;
    [Header("Extra")]
    public float spin;
    public string[] tagList = { "Enemy", "Object" };


    public void shoot()
    {
        rb.AddForce(transform.right * projectileforce * rb.mass);
        //print(transform.forward * projectileforce * rb.mass);
        if (spin != 0)
        {
            rb.AddTorque(spin);
        }
        

    }

    private void Update()
    {
        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            timeToLive -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tagList.Contains(collision.tag))
        {
            collision.GetComponent<EnemyScript>().takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
