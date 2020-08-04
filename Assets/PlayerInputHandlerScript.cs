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
    public PlayerInput playerInputComponent;
    [Header("AI")]
    public bool AI = false;
    public List<EventType> currentEvents = new List<EventType>();
    public List<EventType> savedEvents = new List<EventType>();

    public Vector2 moveDir;
    public Vector2 mousePosition;
    public bool isFiring = false;
    public bool isUsing = false;
    public bool isReloading = false;
    public float startTime;
    public int currentEventPointer;
    [SerializeField] float timeNow_playBack;
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

    private void Start()
    {
        newEvent();
    }


    private void Update()
    {
        /*
        kb = InputSystem.GetDevice<Keyboard>();
        if (kb.pKey.isPressed)
        {
            activeAI(false);
        }
        else if (kb.oKey.isPressed)
        {
            activeAI(true);

        }
        */

        //events = new List<EventType>();
        getMousePosition();

        if (AI)
        {
            playBackEvents();
        }
    }
    public void movePlayer(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
        print("moving player " + moveDir);
        playerMovementScript.playerControls(moveDir);
        playerMovementScript.aimWeapon(mousePosition);

        recordEvent(context);
    }

    public void shoot(InputAction.CallbackContext context)
    {
        mousePosition = Mouse.current.position.ReadValue();
        //playerMovementScript.aimWeapon(mousePosition);
        weaponTypeScript.toggleFiring(context.performed);
        isFiring = context.performed;
        recordEvent(context);

    }
    public void useWeapon(InputAction.CallbackContext context)
    {
        mousePosition = Mouse.current.position.ReadValue();
        UseSecondaryScript.toggleUse(context);
        isUsing = context.performed;
        recordEvent(context);


    }


    public void reload(InputAction.CallbackContext context)
    {
        weaponTypeScript.reload();
        isReloading = context.performed;
        recordEvent(context);

    }
    public void newEvent()
    {
        endEvent();
        currentEvent = new EventType(moveDir, getMousePosition());


    }

    public void endEvent()
    {
        if (currentEvent != null)
        {
            currentEvent.endLog();
            currentEvents.Add(currentEvent);
        }
        currentEvent = null;
    }

    public void recordEvent(InputAction.CallbackContext context)
    {
        newEvent();
        //print(context.action.actionMap.ToString());
        if (moveDir.magnitude > 0)
        {

            //print("Logging move");

            currentEvent.addLog("Move");
        }
        if (isFiring)
        {
            //print("Logging shoot");
            currentEvent.addLog("Shoot");
        }

        if (isUsing)
        {
            currentEvent.addLog("Use");
        }

        if (isReloading)
        {
            currentEvent.addLog("Reload");
        }


    }

    public void activeAI(bool b)
    {
        if (AI != b)
        {
            AI = b;

            playerMovementScript.AI = AI;
            if (AI)
            {
                endEvent();
                resetEvent();
                replayEvents();
                playerInputComponent.enabled = false;
            }
            else
            {
                resetEvent();
                newEvent();
                playerInputComponent.enabled = true;
                currentEvents = new List<EventType>();
            }
        }
        else
        {
            if (AI)
            {
                replayEvents();
            }
        }

    }

    public void setEventList(List<EventType> e)
    {
        savedEvents = e;
        startTime = Time.time;

    }

    public void replayEvents()
    {
        setEventList(currentEvents);

    }

    public void playBackEvents()
    {
        if (currentEventPointer >= savedEvents.Count)
        {
            //print(name + " event empty");
            return;
        }
        currentEvent = savedEvents[currentEventPointer];
        print(currentEvent);
        if (Time.time - startTime < currentEvent.duration)
        {
            playEvent(currentEvent);
        }
        else
        {
            startTime = Time.time;
            currentEventPointer++;
        }
    }


    public void playEvent(EventType et)
    {
        //print(et);
        string s;
        playerMovementScript.playerControls(new Vector2(0, 0));
        //playerMovementScript.aimWeapon(et.mouseLocation);

        foreach (LogType l in et.logs)
        {
            s = l.inputType;
            print("Replaying " +s+ (et.moveDir) + (et.mouseLocation));
            if (s.Equals("Move"))
            {

                playerMovementScript.playerControls(et.moveDir);
                //playerMovementScript.aimWeapon(et.mouseLocation);
            }
            else if (s.Equals("Shoot"))
            {
                playerMovementScript.aimWeapon(et.mouseLocation);

                weaponTypeScript.fireWeapon();
            } else if (s.Equals("Reload"))
            {
                weaponTypeScript.reload();
            }
        }
    }

    void resetEvent()
    {
        startTime = Time.time;
        currentEvent = null;
    }


    public void Rewind()
    {
        currentEventPointer = 0;
        weaponTypeScript.Rewind();
    }

    Vector2 getMousePosition()
    {
        mousePosition = Mouse.current.position.ReadValue() - new Vector2(Screen.width / 2, Screen.height / 2);

        mousePosition = Mouse.current.position.ReadValue() - new Vector2(Screen.width / 2, Screen.height / 2);
        return mousePosition;
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
