using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float maxHealth;
    [SerializeField] float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
            Destroy(gameObject);
        }
    }

    public float takeDamage(float damage)
    {
        currentHealth -= damage;
        return currentHealth;
    }
}
