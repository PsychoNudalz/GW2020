using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    public GameObject projectile;
    public WeaponTypeScript weaponTypeScript;
    //public float timeBetweenShoot;

    // Start is called before the first frame update


    public void shoot()
    {
        weaponTypeScript.fireWeapon();
    }
}
