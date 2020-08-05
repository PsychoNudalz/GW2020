using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth = 100f;
    public float currentHealth;
    public Rigidbody2D rb;


    void Start()
    {
        currentHealth = maxHealth;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkDie();

    }


    void checkDie()
    {
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            //enemySpawnWaveHandler.Enemies.Remove(gameObject);
        }
    }
    public float takeDamage(float damage)
    {
        currentHealth -= damage;
        return currentHealth;
    }

    public void Rewind()
    {
        currentHealth = maxHealth;
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
    }
}
