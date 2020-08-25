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
    public bool immune = false;
    public bool canRegen = false;
    public float regenAmount = 10f;

    [Header("Damage PopUp")]
    public DamagePopUpPoolScript damagePopUpPoolScript;

    [Header("Player Finder")]
    public bool stationary = false;
    public GameObject Player;
    public GameObject[] PlayerPool;
    public AIDestinationSetter des;
    public Seeker seeker;
    public float rangeToFindPlayer;
    [SerializeField] bool inSight;
    [SerializeField] Vector3 playerLastPosition;
    [SerializeField] LayerMask layerMask;

    [Header("Spawn")]
    [SerializeField] EnemySpawnWaveHandler enemySpawnWaveHandler;

    [Header("Colour Change")]
    public bool canChangeColour;
    public SpriteRenderer sprite;
    public Color targetColour;

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
        if (canRegen)
        {
            regenHealth();
        }
        if (!stationary)
        {
            if (des.target == null)
            {
                walkToTarget();

            }
            else if (!des.target.Equals(Player))
            {
                walkToTarget();

            }

        }

    }

    void checkDie()
    {
        if (currentHealth <= 0 && !immune)
        {
            playSound_Death();
            gameObject.SetActive(false);
            //enemySpawnWaveHandler.Enemies.Remove(gameObject);
        }
    }

    public float takeDamage(float damage)
    {
        playSound_Hit();
        if (currentHealth > -1)
        {
            currentHealth -= damage;

        }
        checkDie();
        try
        {
        damagePopUpPoolScript.newDamageValue(damage);
        updateColour();

        } catch(System.Exception e)
        {
            Debug.LogError(name + " Failed to display damage");
        }

        return currentHealth;
    }



    public bool walkToTarget()
    {
        if (Player == null)
        {
            return false;
        }

        if (!inSight)
        {
            //print(name + " cant see player moving");
            des.target = Player.transform;
            //seeker.
            return true;
        }
        else
        {
            des.target = transform;
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
            if (p.activeSelf)
            {
                dis = (transform.position - p.transform.position).magnitude;
                if (dis < closestDis)
                {
                    closestDis = dis;
                    Player = p;
                }

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

    void regenHealth()
    {
        if (currentHealth < maxHealth && canRegen)
        {
            currentHealth += regenAmount * Time.deltaTime;
            updateColour();
        }
    }

    void updateColour()
    {
        if (canChangeColour)
        {
            Color temp = (targetColour * (1 - currentHealth / maxHealth));
            sprite.color = new Color(temp.r, temp.g, temp.b, 1);

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
