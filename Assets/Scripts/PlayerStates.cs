using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    [Header("Max States")]
    public float MaxHealth = 100;
    [Header("Current States")]
    public float currentHealth;
    public bool isDead = false;
    [Header("Killer")]
    public GameObject killer;
    [Header("Sound")]
    public SoundManager soundManager;
    public Sound sound_Death;
    public Sound sound_Hit;
    public Sound sound_Theme;

    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
        soundManager = FindObjectOfType<SoundManager>();

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
        playSound_Hit();

        currentHealth -= damage;
        checkDie();
        return currentHealth;
    }
    void checkDie()
    {
        if (currentHealth <= 0)
        {
            playSound_Death();
            gameObject.SetActive(false);
            isDead = true;
            //enemySpawnWaveHandler.Enemies.Remove(gameObject);
        }
    }

    public void Rewind()
    {
        gameObject.SetActive(true);

        currentHealth = MaxHealth;
        isDead = false;
    }

    void playSound_Hit()
    {
        soundManager.Play(sound_Hit.name);
    }

    void playSound_Death()
    {
        soundManager.Play(sound_Death.name);

    }
}
