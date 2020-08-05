using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    [Header("Max States")]
    public float MaxHealth = 100;
    [Header("Current States")]
    public float currentHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getHeathUI()
    {
        return "Health: "+currentHealth;
    }
    public float takeDamage(float damage)
    {
        currentHealth -= damage;
        return currentHealth;
    }
}
