﻿using System.Collections;
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
    public float reloadTime;
    [SerializeField] private float timeNow_reload;
    public bool isReloading;
    public bool autoload = true;
    public float autoloadRate = 10f;
    [Header("Fire Behaviour")]
    public float rpm;
    public float spreadIncreaseRate;
    public float spreadDecreaseRate;
    public int projectilePerShot;
    public float spreadAngle;
    public bool spreadDebug;
    [SerializeField] private float currentSpread = 0;
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private float timeNow_rpm;
    public float TimeNow { get => timeNow_rpm; set => timeNow_rpm = value; }

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
        if (timeNow_rpm > 0)
        {
            timeNow_rpm -= Time.deltaTime;

        }

        if (currentMag == 0)
        {
            reload();
        }

        //print(canFire());
        if (isReloading)
        {
            print("reloading");
            reload();
        }
        if (currentSpread > 0)
        {
            currentSpread -= spreadDecreaseRate * Time.deltaTime;
        }
        else if (currentSpread < 0)
        {
            currentSpread = 0;
        }

        if (spreadDebug)
        {
            //Debug
            Quaternion randomSpread1 = Quaternion.AngleAxis(currentSpread, Vector3.forward);
            Quaternion randomSpread2 = Quaternion.AngleAxis(-currentSpread, Vector3.forward);
            Debug.DrawRay(transform.position, randomSpread1 * transform.up * 10f, Color.green, 0f);
            Debug.DrawRay(transform.position, randomSpread2 * transform.up * 10f, Color.green, 0f);


        }
    }

    public bool canFire()
    {
        if (timeNow_rpm <= 0 && !isReloading)
        {


            if (currentMag >= Ammo_Cost)
            {
                timeNow_rpm = timeBetweenShot;

                return true;
            }
            else
            {
                return false;
            }

        }
        print("out1");
        return false;


    }

    public bool isFullAmmo()
    {
        bool full = Ammo >= Ammo_Max;
        return full;
    }
    public bool isFullClip()
    {
        bool full = currentMag >= Ammo_Max;
        return full;
    }

    public bool consumeAmmo()
    {
        if (canFire())
        {
            currentMag -= Ammo_Cost;
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
                    pistolShot();
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

    public bool reload()
    {
        if (currentMag >= maxInMag)
        {
            return false;
        }
        currentSpread = 0;
        if (!isReloading)
        {
            timeNow_reload = reloadTime;
        }
        isReloading = true;
        //animator.SetBool("Reload", false);
        if (timeNow_reload > 0)
        {
            //animator.SetBool("Reload", true);

            timeNow_reload -= Time.deltaTime;
            return false;
        }
        else
        {
            //animator.SetBool("Reload", false);
            isReloading = false;
            Ammo -= maxInMag - currentMag;
            currentMag = maxInMag;
            return false;
        }
    }


    //Weapon Fire

    void shotgunShot()
    {
        GameObject projectile;
        Quaternion randomSpread;
        for (int i = 0; i < projectilePerShot; i++)
        {
            randomSpread = Quaternion.AngleAxis(Random.Range(-spreadAngle, spreadAngle), Vector3.forward);
            projectile = Instantiate(shootingProjectile, shootingPoint.transform.position, randomSpread * shootingPoint.transform.rotation);
            projectile.GetComponent<ProjectileScript>().shoot();
            print("fire " + i);
        }
    }

    void pistolShot()
    {
        if (currentSpread > spreadAngle)
        {
            currentSpread = spreadAngle - spreadIncreaseRate;
        }
        GameObject projectile;
        Quaternion randomSpread;
        randomSpread = Quaternion.AngleAxis(Random.Range(-currentSpread, currentSpread), Vector3.forward);
        projectile = Instantiate(shootingProjectile, shootingPoint.transform.position, randomSpread * shootingPoint.transform.rotation);
        projectile.GetComponent<ProjectileScript>().shoot();
        currentSpread += spreadIncreaseRate;


    }

}