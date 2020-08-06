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
    public bool checkDie()
    {
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            isDead = true;
            //enemySpawnWaveHandler.Enemies.Remove(gameObject);
        }
        return isDead;
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
        print("play death sound");

        soundManager.Play(sound_Death.name);

    }
}
