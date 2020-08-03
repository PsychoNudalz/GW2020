using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyAttackScript enemyAttackScript;
    [Header("States")]
    public float maxHealth;
    [SerializeField] float currentHealth;
    [Header("Player Finder")]
    public GameObject Player;
    public float rangeToFindPlayer;
    public bool shareProjectileLayerMask;
    [SerializeField] LayerMask layerMask;
    [Header("Spawn")]
    [SerializeField] EnemySpawnWaveHandler enemySpawnWaveHandler;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (shareProjectileLayerMask)
        {

        }
        
        
        
        currentHealth = maxHealth;
        try
        {
            enemySpawnWaveHandler = GetComponentInParent<EnemySpawnWaveHandler>();
            if (!enemySpawnWaveHandler.Enemies.Contains(gameObject))
            {
                gameObject.SetActive(false);
                enemySpawnWaveHandler.Enemies.Add(gameObject);
            }
        } catch (System.Exception e)
        {

        }



    }

    private void Awake()
    {
        
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
            enemySpawnWaveHandler.Enemies.Remove(gameObject);
        }
    }

    public float takeDamage(float damage)
    {
        currentHealth -= damage;
        return currentHealth;
    }

    void getProjectileLayerMask()
    {

    }
    
}
