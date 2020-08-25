using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float ammoPickupModifier = 2f;
    [Header("Reload")]
    public float reloadTime;
    [SerializeField] private float timeNow_reload;
    public bool isReloading;
    public bool autoload = false;
    public float autoloadRate = 10f;
    [Header("Fire Behaviour")]
    [SerializeField] bool isFiring = false;
    public float rpm;
    public float spreadIncreaseRate;
    public float spreadDecreaseRate;
    public int projectilePerShot;
    public float spreadAngle;
    public bool nonRandomSpread = false;
    public float spreadMultiplierPrime = 97;
    public bool spreadDebug;
    [SerializeField] Quaternion randomSpread;
    [SerializeField] Quaternion firedDirection;
    [SerializeField] private float currentSpread = 0;
    [SerializeField] private float timeBetweenShot;
    [SerializeField] private float timeNow_rpm;
    public float TimeNow { get => timeNow_rpm; set => timeNow_rpm = value; }

    [Header("Sound")]
    public SoundManager soundManager;
    public Sound sound_Fire;


    [Header("PlayerInputHandler")]
    public PlayerInputHandlerScript playerInputHandlerScript;

    //public bool fireWhenFull;
    //[SerializeField] private bool full;
    //[SerializeField] private bool activeFire;
    //public bool ActiveFire { get => activeFire; set => activeFire = value; }

    private void Awake()
    {
        timeBetweenShot = 60f / rpm;
        soundManager = FindObjectOfType<SoundManager>();
        playerInputHandlerScript = GetComponentInParent<PlayerInputHandlerScript>();

    }

    private void Update()
    {
        if (isFiring)
        {
            fireWeapon();
        }

        if (timeNow_rpm > 0)
        {
            timeNow_rpm -= Time.deltaTime;
            if (currentMag == 0)
            {
                reload();
            }
        }



        //print(canFire());
        if (isReloading)
        {
            //print("reloading");
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


        }

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

    public void toggleFiring(bool context)
    {
        isFiring = context;
        //print("toggle Firing: " + isFiring);
    }

    public void fireWeapon()
    {
        if (consumeAmmo())
        {
            playSound_Fire();
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


            }
        }
        else
        {
            //activeFire = false;
        }
        //print("shoot at "+transform.up);
    }

    public void fireWeaponForced(Quaternion dir)
    {
        playSound_Fire();
        //playerInputHandlerScript.recordShootEvent();
        //activeFire = true;
        switch (weapon)
        {
            case WeaponEnum.Shotgun:
                shotgunShot();
                //launcher.GetComponent<MissileLaunchPointScript>().fire();
                break;
            case WeaponEnum.Pistol:
                pistolShot(dir);
                //launcher.GetComponent<TrapLaunchPointScript>().fire();
                break;


        }
    }

    public void fireWeapon(GameObject s, GameObject t)
    {
        if (consumeAmmo())
        {
            playSound_Fire();
            //activeFire = true;
            switch (weapon)
            {
                case WeaponEnum.EnemyWeapon:
                    enemyShot(s, t);
                    break;

            }
        }
        else
        {
            //activeFire = false;
        }
        //print("shoot at "+transform.up);
    }



    public void addAmmo(float amout)
    {
        ammo += amout * ammoPickupModifier;
        print("Added " + amout + ", increased to " + ammo);
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

    public void reload()
    {
        if (currentMag >= maxInMag || Ammo == 0)
        {
            return;
        }
        currentSpread = 0;
        if (!isReloading)
        {
            timeNow_reload = reloadTime;
            animator.SetTrigger("Reload");
        }
        isReloading = true;

        if (timeNow_reload > 0)
        {

            timeNow_reload -= Time.deltaTime;
            return;
        }
        else
        {
            isReloading = false;
            if (Ammo + currentMag < maxInMag)
            {
                currentMag += Ammo;
                Ammo = 0;
            }
            else
            {
                Ammo -= maxInMag - currentMag;
                currentMag = maxInMag;

            }
            return;
        }
    }

    /*
    public LayerMask getLayermask()
    {
        return shootingProjectile.
    }

    */

    public void Rewind()
    {
        Ammo = Ammo_Max;
        currentMag = maxInMag;
        isFiring = false;
        isReloading = false;
    }


    //Weapon Fire

    void shotgunShot()
    {
        GameObject projectile;
        //Quaternion randomSpread;
        for (int i = 0; i < projectilePerShot; i++)
        {
            //randomSpread = Quaternion.AngleAxis(Random.Range(-spreadAngle, spreadAngle), Vector3.forward);
            randomSpread = Quaternion.AngleAxis(-spreadAngle + i * (spreadAngle * 2) / projectilePerShot, Vector3.forward);
            print(name + "  " + i + "  " + randomSpread.eulerAngles);
            projectile = Instantiate(shootingProjectile, shootingPoint.transform.position, randomSpread * shootingPoint.transform.rotation);
            projectile.GetComponent<ProjectileScript>().shoot();
            print("fire " + i);
        }
        playerInputHandlerScript.recordEvent(true);

    }

    void pistolShot()
    {
        //animator.SetBool("Shoot", true);
        animator.SetTrigger("Shoot");

        GameObject projectile;
        //Quaternion randomSpread;
        if (!nonRandomSpread || currentSpread < 1f)
        {
            randomSpread = Quaternion.AngleAxis(Random.Range(-currentSpread, currentSpread), Vector3.forward);
        }
        else
        {
            float tempRandomValue = (currentMag * spreadMultiplierPrime % (currentSpread * 2)) - currentSpread;
            randomSpread = Quaternion.AngleAxis(tempRandomValue, Vector3.forward);

        }
        firedDirection = randomSpread * shootingPoint.transform.rotation;

        projectile = Instantiate(shootingProjectile, shootingPoint.transform.position, firedDirection );

        projectile.GetComponent<ProjectileScript>().shoot();
        currentSpread += spreadIncreaseRate;
        if (currentSpread > spreadAngle)
        {
            currentSpread = spreadAngle;
        }
        playerInputHandlerScript.recordEvent(true, firedDirection);


    }
    void pistolShot(Quaternion dir)
    {
        //animator.SetBool("Shoot", true);
        animator.SetTrigger("Shoot");

        GameObject projectile;
        firedDirection = dir;

        projectile = Instantiate(shootingProjectile, shootingPoint.transform.position, firedDirection);

        projectile.GetComponent<ProjectileScript>().shoot();

        //playerInputHandlerScript.recordEvent(true);


    }




    void enemyShot(GameObject s, GameObject t)
    {

        animator.SetTrigger("Shoot");

        if (currentSpread > spreadAngle)
        {
            currentSpread = spreadAngle - spreadIncreaseRate;
        }
        GameObject projectile;
        //Quaternion randomSpread;
        randomSpread = Quaternion.AngleAxis(Random.Range(-currentSpread, currentSpread), Vector3.forward);
        projectile = Instantiate(shootingProjectile, shootingPoint.transform.position, randomSpread * shootingPoint.transform.rotation);
        projectile.GetComponent<ProjectileScript>().setShooter(s);
        projectile.GetComponent<ProjectileScript>().setTarget(t);
        projectile.GetComponent<ProjectileScript>().shoot();

        currentSpread += spreadIncreaseRate;

    }
    void playSound_Fire()
    {
        soundManager.Play(sound_Fire.name);

    }



}
