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

    // Start is called before the first frame update
    void Start()
    {
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

    void getProjectileLayerMask()
    {

    }

    bool checkPlayerInsight()
    {
        Vector3 dir = (Player.transform.position - transform.position).normalized;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, dir, rangeToFindPlayer, layerMask);
        Debug.DrawRay(transform.position, (dir) * rangeToFindPlayer, Color.green);

        if (hit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerLastPosition = Player.transform.position;
                return true;
            }
            return false;
        }
        return false;
    }
    void updatePlayerPosition()
    {
        double dis;
        double closestDis = double.PositiveInfinity;
        foreach (GameObject p in PlayerPool)
        {
            dis = (transform.position - p.transform.position).magnitude;
            if ( dis < closestDis)
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

            enemyAttackScript.shoot((playerLastPosition - transform.position).normalized);
        }
    }

    public void Rewind()
    {
        currentHealth = maxHealth;

    }
}
