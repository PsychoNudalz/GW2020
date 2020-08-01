using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypeScript : MonoBehaviour
{
    [SerializeField] private WeaponEnum weapon;
    //[SerializeField] private ShootingScript shootingScript;
    public GameObject shootingProjectile;
    public GameObject shootingPoint;
    public Animator animator;


    //public GameObject aimerGameObject;
    //[SerializeField] private GameObject aimer;

    [Header("Ammo")]
    [SerializeField] private float ammo;
    public float Ammo { get => ammo; set => ammo = value; }
    public float Ammo_Max;
    public float Ammo_Cost = 10;
    public float maxInMag;
    public float currentMag;
    public bool autoload = true;
    public float autoloadRate = 10f;
    [Header("Fire Behaviour")]
    public float rpm;
    public int projectilePerShot;
    public float spreadAngle;
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private float timeNow;
    public float TimeNow { get => timeNow; set => timeNow = value; }

    //public bool fireWhenFull;
    //[SerializeField] private bool full;
    //[SerializeField] private bool activeFire;
    //public bool ActiveFire { get => activeFire; set => activeFire = value; }

    private void Awake()
    {
        timeBetweenShot = 60f / rpm;

    }

    private void Update()
    {
        animator.SetBool("Reload", false);
        if (timeNow > 0)
        {
            animator.SetBool("Reload", true);

            timeNow -= Time.deltaTime;
        }
    }

    public bool canFire()
    {
        if (timeNow <= 0)
        {
            if (isFull())
            {
                timeNow = timeBetweenShot;

                return true;
            }

            if (ammo >= Ammo_Cost)
            {
                timeNow = timeBetweenShot;

                return true;
            }
            else
            {
                return false;
            }

        }
        return false;


    }

    public bool isFull()
    {
        bool full = Ammo >= Ammo_Max;
        return full;
    }

    public bool consumeAmmo()
    {
        if (canFire())
        {
            ammo -= Ammo_Cost;
            return true;
        }
        return false;
    }
    public void fireWeapon()
    {
        if (consumeAmmo())
        {
            //activeFire = true;
            switch (weapon)
            {
                case WeaponEnum.Shotgun:
                    shotgunShot();
                    //launcher.GetComponent<MissileLaunchPointScript>().fire();
                    break;
                case WeaponEnum.Pistol:
                    //launcher.GetComponent<TrapLaunchPointScript>().fire();
                    break;
                case WeaponEnum.Claw:
                    break;
            }
        }
        else
        {
            //activeFire = false;
        }
        //print(ammo);

    }

    public void addAmmo(float amout)
    {
        ammo += amout;
    }

    public string getWeaponType()
    {
        string name = "NULL";
        switch (weapon)
        {
            case WeaponEnum.Shotgun:
                name = "Shotgun";
                break;
            case WeaponEnum.Pistol:
                name = "Pistol";
                break;
            case WeaponEnum.Claw:
                break;
        }
        return name;
    }


    //Weapon Fire

    void shotgunShot()
    {
        GameObject projectile;
        Quaternion randomSpread;
        for (int i = 0; i < projectilePerShot; i++)
        {
            randomSpread = Quaternion.AngleAxis(Random.Range(-spreadAngle, spreadAngle), Vector3.forward);
            projectile = Instantiate(shootingProjectile, shootingPoint.transform.position, randomSpread*shootingPoint.transform.rotation);
            projectile.GetComponent<ProjectileScript>().shoot();
            print("fire "+i);
        }
    }


}
