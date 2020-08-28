using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript : MonoBehaviour
{
    public GameObject prefab;
    [Header("YEEEET! values")]
    public Rigidbody2D rb;
    public float force;
    public float spin;
    [Header("Collision Damage")]
    public float damage;
    public float objectDamageMultiplier = 1f;
    public float minVelocity;
    [SerializeField] List<GameObject> hitRecord = new List<GameObject>();
    [Header("Explosive")]
    public bool isExplosive = false;
    public GameObject explosiveClusters;
    public int clusterAmount;
    [SerializeField] bool isArmed = false;
    [Header("TripMine")]
    public float detectRange;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

    }



    public void YEET()
    {
        hitRecord = new List<GameObject>();
        rb.AddForce(transform.up * force);
        rb.AddTorque(spin);
        print(name + " Yeet ");
    }

    public void Rewind()
    {
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.magnitude > minVelocity)
        {
            if (!hitRecord.Contains(collision.gameObject))
            {
                hitRecord.Add(collision.gameObject);
                if (collision.collider.CompareTag("Enemy"))
                {
                    EnemyScript e1;
                    if (collision.gameObject.TryGetComponent<EnemyScript>(out e1))
                    {
                        print(name + " hit " + collision.gameObject.name);
                        e1.takeDamage(damage);

                    }

                }
                else if (collision.collider.CompareTag("MovableObject"))
                {
                    DestructableScript e1;
                    if (collision.gameObject.TryGetComponent<DestructableScript>(out e1))
                    {
                        print(name + " hit " + collision.gameObject.name);
                        e1.takeDamage(damage * objectDamageMultiplier);

                    }
                }
            }
        }

        if (isExplosive)
        {

        }

    }


    public void detonate()
    {

    }

}
