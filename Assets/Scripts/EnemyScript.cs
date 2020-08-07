using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public EnemyAttackScript enemyAttackScript;
    [Header("States")]
    public float maxHealth;
    [SerializeField] float currentHealth;
    [Header("Player Finder")]
    public GameObject Player;
    public GameObject[] PlayerPool;
    public AIDestinationSetter des;
    public float rangeToFindPlayer;
    [SerializeField] bool inSight;
    [SerializeField] Vector3 playerLastPosition;
    [SerializeField] LayerMask layerMask;

    [Header("Spawn")]
    [SerializeField] EnemySpawnWaveHandler enemySpawnWaveHandler;

    [Header("Sound")]
    public SoundManager soundManager;
    public Sound sound_Death;
    public Sound sound_Hit;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        PlayerPool = GameObject.FindGameObjectsWithTag("Player");
        /*
        /*
        if (shareProjectileLayerMask)
        {

        }
        */
        updatePlayerPosition();
        currentHealth = maxHealth;
        try
        {
            enemySpawnWaveHandler = GetComponentInParent<EnemySpawnWaveHandler>();
            if (!enemySpawnWaveHandler.Enemies.Contains(gameObject))
            {
                gameObject.SetActive(false);
                enemySpawnWaveHandler.Enemies.Add(gameObject);
            }
        }
        catch (System.Exception e)
        {

        }



    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updatePlayerPosition();
        inSight = checkPlayerInsight();
        shootOnShot();
        if (des.target == null)
        {
            walkToTarget();

        }

    }

    void checkDie()
    {
        if (currentHealth <= 0)
        {
            playSound_Death();
            gameObject.SetActive(false);
            //enemySpawnWaveHandler.Enemies.Remove(gameObject);
        }
    }

    public float takeDamage(float damage)
    {
        playSound_Hit();
        currentHealth -= damage;
        checkDie();

        return currentHealth;
    }



    public bool walkToTarget()
    {
        if (!inSight && !des.target.Equals(Player))
        {
            print(name + " cant see player moving");
            des.target = Player.transform;
            return true;
        }
        else
        {
            des.target = null;
            return false;
        }
    }

    bool checkPlayerInsight()
    {
        Vector3 dir = (Player.transform.position - transform.position).normalized;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, dir, rangeToFindPlayer, layerMask);

        if (hit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, (dir) * rangeToFindPlayer, Color.green);
                playerLastPosition = Player.transform.position;
                return true;
            }
        }

            Debug.DrawRay(transform.position, (dir) * rangeToFindPlayer, Color.red);
        return false;
    }
    void updatePlayerPosition()
    {
        double dis;
        double closestDis = double.PositiveInfinity;
        foreach (GameObject p in PlayerPool)
        {
            dis = (transform.position - p.transform.position).magnitude;
            if (dis < closestDis)
            {
                closestDis = dis;
                Player = p;
            }
        }
    }

    void shootOnShot()
    {
        if (inSight)
        {
            enemyAttackScript.shoot((playerLastPosition - transform.position).normalized, gameObject, Player);
        }
    }

    public void Rewind()
    {
        currentHealth = maxHealth;
        //print(name + " heath " + currentHealth);
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
