using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript : MonoBehaviour
{
    public SpriteRenderer renderer;
    public float decayValue = 1f;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (renderer.material.GetFloat("_Outline") > 0f)
        {
            renderer.material.SetFloat("_Outline", renderer.material.GetFloat("_Outline") - Time.deltaTime * decayValue);
        }
        //print(renderer.material.GetFloat("_Outline"));
    }

    public void setOutline(float f)
    {
        renderer.material.SetFloat("_Outline", f);
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

    }

}
