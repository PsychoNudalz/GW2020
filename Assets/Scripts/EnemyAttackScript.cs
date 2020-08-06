using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    public GameObject projectile;
    public WeaponTypeScript weaponTypeScript;
    //public float timeBetweenShoot;

    // Start is called before the first frame update


    public void shoot(Vector3 v)
    {
        weaponTypeScript.shootingPoint.transform.up = v;
        weaponTypeScript.fireWeapon();
    }
    public void shoot(Vector3 v,GameObject s, GameObject t)
    {
        print("Enemy shoot");
        weaponTypeScript.shootingPoint.transform.up = v;
        weaponTypeScript.fireWeapon(s,t);
    }
}
