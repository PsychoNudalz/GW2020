using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.U2D.Path.GUIFramework;
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
    public List<EventType> events = new List<EventType>();
    public Vector2 moveDir;
    public Vector2 mousePosition;
    Keyboard kb;
    Mouse m;
    Pointer p;
    [SerializeField] EventType currentEvent;

    // Start is called before the first frame update
    private void Awake()
    {
        controls = new InputMaster();
        /*
        controls.Player.Movement.performed += ctx => movePlayer(ctx.ReadValue<Vector2>());
        controls.Player.Shoot.performed += ctx => shoot();
        controls.Player.Use.performed += ctx => useWeapon(ctx.ReadValueAsButton());
        controls.Player.Reload.performed += ctx => reload();
        */
    }

   
    private void Update()
    {
        events = new List<EventType>();
        if (!AI)
        {
            kb = InputSystem.GetDevice<Keyboard>();
            m = InputSystem.GetDevice<Mouse>();
            p = InputSystem.GetDevice<Pointer>();
            //recordEvent(kb.IsPressed());
        }
    }
    public void movePlayer(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
        playerMovementScript.playerControls(moveDir);
    }

    public void shoot(InputAction.CallbackContext context)
    {
        mousePosition = Mouse.current.position.ReadValue();
        weaponTypeScript.toggleFIring(context);
    }
    public void useWeapon(InputAction.CallbackContext context)
    {
        mousePosition = Mouse.current.position.ReadValue();
        UseSecondaryScript.toggleUse(context);

    }


    public void reload(InputAction.CallbackContext context)
    {
        weaponTypeScript.reload();
    }


    public void recordEvent(InputAction.CallbackContext context)
    {
        if (currentEvent != null)
        {
            currentEvent.endLog();
        }
        currentEvent = new EventType(Mouse.current.position.ReadValue());
        if (controls.Player.Shoot.enabled)
        {
            //print("recorder detected");
        }
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
