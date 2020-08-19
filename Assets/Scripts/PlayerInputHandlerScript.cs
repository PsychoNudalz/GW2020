using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

//Takes player inputs and records
public class PlayerInputHandlerScript : MonoBehaviour
{
    public InputMaster controls;
    public Camera cam;

    //[SerializeField]private InputActionAsset inputActions;
    [Header("Scripts")]
    public TimeManagerScript timeManager;
    public PlayerMovementScript playerMovementScript;
    public PlayerStates playerStates;
    public WeaponTypeScript weaponTypeScript;
    public UseSecondaryScript useSecondaryScript;
    [Header("AI")]
    public bool AI = false;
    public bool isStartPlayBack = false;
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
    [SerializeField] float currentTime;
    [SerializeField] EventType currentEvent;
    [SerializeField] int currentCounter;
    [SerializeField] int savedCounter;
    [SerializeField] bool isCurrentEventEmpty;

    // Start is called before the first frame update
    private void Awake()
    {
        controls = new InputMaster();
        timeManager = FindObjectOfType<TimeManagerScript>();
        cam = FindObjectOfType<Camera>();
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
        //getMousePosition();
        if (!AI)
        {
            getMousePosition();
            playerMovementScript.setMousePosition(mousePosition);
        }


        currentTime = timeManager.currentTime;

        if (AI && isStartPlayBack)
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
        currentEvent = new EventType(moveDir, getMousePosition(),currentTime);


    }

    public void endEvent()
    {
        if (currentEvent != null)
        {
            currentEvent.endLog(transform.position,currentTime);
            savedEvents.Add(currentEvent);
        }
        currentEvent = null;
    }

    public void endRecording()
    {
        endEvent();
        //add empty event
        ///*
        currentEvent = new EventType(new Vector2(), new Vector2(1000, 10000),currentTime);
        currentEvent.endLog(transform.position, currentTime);
        savedEvents.Add(currentEvent);
        //*/
        currentEvent = null;
        resetRecordPara();

    }

    public void startRecording()
    {
        currentEvent = null;
        currentEvents = new List<EventType>();
        savedEvents = new List<EventType>();
        newEvent();
    }

    public void recordEvent()
    {
        //print(name + " new record");
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
        //resetRecordRewind();

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
                startRecording();
            }
        }

        AI = b;

        playerMovementScript.AI = AI;
        useSecondaryScript.AI = AI;
        if (AI)
        {
            loadEventList();
            //playerInputComponent.enabled = false;
        }
        else
        {
            //startTime = currentTime;
            //newEvent();
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

        startTime = currentTime;

    }
    /*
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
    */

    public void loadEventList()
    {
        currentEvents = new List<EventType>();
        print(name + " loading " + savedEvents.Count + " Events");
        //savedEvents = e;
        foreach (EventType eventType in savedEvents)
        {
            currentEvents.Add(eventType);
        }

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
            resetRecordPara();
            playEvent(new EventType(new Vector2(0, 0), new Vector2(0, 0), currentTime));
            //print(name + " event empty");
            return;
        }
        currentEvent = currentEvents[currentEventPointer];
        if (currentEvent == null)
        {
            print(name + " currentEvent is null");
            return;
        }
        if (currentTime < currentEvent.timeEnd)
        {
            playEvent(currentEvent);
        }
        else
        {
            //when current event finish
            setEndPosition(currentEvent.characterLocation);
            startTime = currentTime;
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
                playerMovementScript.hardAimWeapon(et.mouseLocation);
                //StartCoroutine(waitForShoot(0.02f));

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

                useSecondaryScript.isUsing = true;
            }
            else if (s.Equals("StopUse"))
            {
                //print("AI stop using");

                useSecondaryScript.isUsing = false;

            }
        }
    }

    IEnumerator waitForShoot(float f)
    {
        print("waiting");
        yield return new WaitForSeconds(f);
        weaponTypeScript.fireWeapon();

        print("wait finish");

    }

    void resetRecordPara()
    {
        //currentEvent = null;
        moveDir = new Vector2(0,0);
        mousePosition = new Vector2(0, 0);
        isFiring = false;
        isUsing = false;
        isReloading = false;
        //startTime = currentTime;
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
        playerStates.Rewind();
        playerMovementScript.playerControls(new Vector3(0, 0, 0));
        //activeAI(true);
        startPlayBack();
        

    }

    Vector2 getMousePosition()
    {
        //mousePosition = Mouse.current.position.ReadValue() - new Vector2(Screen.width / 2, Screen.height / 2);
        mousePosition = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return mousePosition;
    }

    void setEndPosition(Vector3 loc)
    {
        transform.position = loc;
        //print(name + " position set to " + loc);
    }

    public void startPlayBack()
    {
        startTime = 0;

        if (!isStartPlayBack)
        {
            isStartPlayBack = true;
            
            //loadEventList();
        }
        if (!AI)
        {
            startRecording();
        }
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        print(name + "death time " + currentTime);
    }

    


}
