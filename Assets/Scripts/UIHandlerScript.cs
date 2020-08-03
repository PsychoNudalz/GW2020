using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandlerScript : MonoBehaviour
{
    [Header("Ammo")]
    public WeaponTypeScript weaponTypeScript;
    public TextMeshProUGUI ammoTextBox;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ammoTextBox.text = weaponTypeScript.currentMag + "/" + weaponTypeScript.Ammo;
    }
}
