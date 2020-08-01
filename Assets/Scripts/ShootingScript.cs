using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [Header("Primary")]
    public WeaponTypeScript weapon;
    [Header("Secondary")]
    public UseSecondaryScript secondary;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Fire1") != 0)
        {
            weapon.fireWeapon();
        }
        if (Input.GetAxisRaw("Fire2") != 0)
        {
            secondary.use();
        }
        else
        {
            secondary.stop();
        }
    }
}
