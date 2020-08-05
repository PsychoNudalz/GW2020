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
    public UseSecondaryScript useSecondaryScript;
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
    [SerializeField] int currentCounter;
    [SerializeField] int savedCounter;
    [SerializeField] bool isCurrentEventEmpty;

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
        //newEvent();
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
        //print(isUsing);
        //events = new List<EventType>();
        getMousePosition();

        if (AI)
        {
            playBackEvents();
        }

        currentCounter = currentEvents.Count;
        savedCounter = savedEvents.Count;
        isCurrentEventEmpty = currentEvent == null;
    }
    public void movePlayer(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
        //print("moving player " + moveDir);
        playerMovementScript.playerControls(moveDir);
        playerMovementScript.setMousePosition(mousePosition);

        recordEvent();
    }

    public void shoot(InputAction.CallbackContext context)
    {
        //mousePosition = Mouse.current.position.ReadValue();
        //playerMovementScript.aimWeapon(mousePosition);
        getMousePosition();
        weaponTypeScript.toggleFiring(context.performed);
        isFiring = context.performed;
        recordEvent();

    }
    public void useWeapon(InputAction.CallbackContext context)
    {
        //mousePosition = Mouse.current.position.ReadValue();
        getMousePosition();
        useSecondaryScript.toggleUse(context);
        isUsing = context.performed;
        recordEvent();


    }


    public void reload(InputAction.CallbackContext context)
    {
        weaponTypeScript.reload();
        isReloading = context.performed;
        recordEvent();

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
            savedEvents.Add(currentEvent);
        }
        currentEvent = null;
    }

    public void endRecording()
    {
        endEvent();
        savedEvents.Add(new EventType(new Vector2(), new Vector2()));
        currentEvent = null;
    }

    public void recordEvent()
    {
        print(name + " new record");
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
            currentEvent.mouseLocation = getMousePosition();
            currentEvent.addLog("Use");
        }
        else
        {
            currentEvent.addLog("StopUse");
        }


        if (isReloading)
        {
            currentEvent.addLog("Reload");
        }


    }

    public void activeAI(bool b)
    {
        resetRewind();

        if (AI != b)
        {
            if (b)
            {
                print(name + " swapping to AI");
                endRecording();
                //saveEventList();

            }
            else
            {
                print(name + " swapping to Player");

            }
        }

        AI = b;

        playerMovementScript.AI = AI;
        useSecondaryScript.AI = AI;
        if (AI)
        {
            loadEventList();
            playerInputComponent.enabled = false;
        }
        else
        {
            playerInputComponent.enabled = true;
            currentEvents = new List<EventType>();
            savedEvents = new List<EventType>();
            //recordEvent();
            newEvent();
        }
        /*
        }
        if (AI != b)
        {
        }
        else
        {
            if (AI)
            {
                replayEvents();
            }
        */
    }

    public void setEventList(List<EventType> e)
    {
        savedEvents = new List<EventType>();
        //savedEvents = e;
        foreach (EventType eventType in e)
        {
            savedEvents.Add(eventType);
        }

        startTime = Time.time;

    }
    public void saveEventList()
    {
        savedEvents = new List<EventType>();
        //savedEvents = e;
        foreach (EventType eventType in currentEvents)
        {
            savedEvents.Add(eventType);
        }

        startTime = Time.time;

    }

    public void loadEventList()
    {
        currentEvents = new List<EventType>();
        print(name + " loading " + savedEvents.Count + " Events");
        //savedEvents = e;
        foreach (EventType eventType in savedEvents)
        {
            currentEvents.Add(eventType);
        }

        startTime = Time.time;
    }

    /*
    public void replayEvents()
    {
        loadEventList();

    }
    */

    public void playBackEvents()
    {
        if (currentEventPointer >= currentEvents.Count)
        {
            //print(name + " event empty");
            return;
        }
        currentEvent = currentEvents[currentEventPointer];
        //print(currentEvent);
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
        //print(name + "Current Event: " + et);
        string s;
        playerMovementScript.playerControls(new Vector2(0, 0));
        playerMovementScript.setMousePosition(et.mouseLocation);
        //StartCoroutine(waitForAim(0.1f));
        foreach (LogType l in et.logs)
        {
            s = l.inputType;
            //print("Replaying " +s+ (et.moveDir) + (et.mouseLocation));
            if (s.Equals("Move"))
            {

                playerMovementScript.playerControls(et.moveDir);
                //playerMovementScript.aimWeapon(et.mouseLocation);
            }
            else if (s.Equals("Shoot"))
            {
                //playerMovementScript.aimWeapon(et.mouseLocation);

                weaponTypeScript.fireWeapon();
            }
            else if (s.Equals("Reload"))
            {
                weaponTypeScript.reload();
            }
            else if (s.Equals("Use"))
            {
                //playerMovementScript.aimWeapon(et.mouseLocation);
                //print("AI using");

                useSecondaryScript.activatingSecondary = true;
            }
            else if (s.Equals("StopUse"))
            {
                //print("AI stop using");

                useSecondaryScript.activatingSecondary = false;

            }
        }
    }

    IEnumerator waitForAim(float f)
    {
        print("waiting");
        yield return new WaitForSeconds(f);
        print("wait finish");

    }

    void resetRewind()
    {
        //currentEvent = null;
        moveDir = new Vector2();
        mousePosition = new Vector2();
        isFiring = false;
        isUsing = false;
        isReloading = false;
        startTime = Time.time;
    }


    public void Rewind()
    {
        //endEvent();
        //print("Rewind to current"+currentEvent);
        //replayEvents();
        //loadEventList();
        currentEventPointer = 0;
        weaponTypeScript.Rewind();
        useSecondaryScript.Rewind();
    }

    Vector2 getMousePosition()
    {
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
