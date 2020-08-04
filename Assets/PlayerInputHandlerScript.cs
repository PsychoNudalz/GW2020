using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

//Takes player inputs and records
public class PlayerInputHandlerScript : MonoBehaviour
{
    //[SerializeField]private InputActionAsset inputActions;
    public InputMaster controls;
    [Header("Scripts")]
    public PlayerMovementScript playerMovementScript;
    public WeaponTypeScript weaponTypeScript;
    public UseSecondaryScript UseSecondaryScript;
    [Header("AI")]
    public bool AI = false;
    Keyboard kb;
    Mouse m;
    Pointer p;

    // Start is called before the first frame update
    private void Awake()
    {

        controls = new InputMaster();
        controls.Player.Movement.performed += ctx => movePlayer(ctx.ReadValue<Vector2>());
        controls.Player.Shoot.performed += ctx => shoot();
        controls.Player.Use.performed += ctx => useWeapon(ctx.ReadValueAsButton());
        controls.Player.Reload.performed += ctx => reload();
    }


    private void Update()
    {
        if (!AI)
        {
            kb = InputSystem.GetDevice<Keyboard>();
            m = InputSystem.GetDevice<Mouse>();
            p = InputSystem.GetDevice<Pointer>();
            //recordEvent(kb.IsPressed());
        }
    }
    void movePlayer(Vector2 v)
    {
        print(v);
        playerMovementScript.playerControls(v);
    }

    void shoot()
    {
        weaponTypeScript.fireWeapon();
    }
    void useWeapon(bool b)
    {
        UseSecondaryScript.use();
    }


    void reload()
    {
        weaponTypeScript.reload();
    }


    public void recordEvent(bool b)
    {

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
