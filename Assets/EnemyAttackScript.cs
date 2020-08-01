using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    public GameObject projectile;
    public float timeBetweenShoot;
    [SerializeField] float timeNow;

    // Start is called before the first frame update
    void Awake()
    {
        timeNow = timeBetweenShoot;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeNow <= 0)
        {
            shoot();
            timeNow = timeBetweenShoot;
        } else {
            timeNow -= Time.deltaTime;
        }
    }

    void shoot()
    {
        ProjectileScript projectileS = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<ProjectileScript>();
        projectileS.shoot();
    }
}
