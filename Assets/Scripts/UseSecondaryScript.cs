using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseSecondaryScript : MonoBehaviour
{
    [Header("Weapon States")]
    [SerializeField] private WeaponEnum weaponEnum;
    public GameObject target;
    public float range;
    [SerializeField] LayerMask layerMask;
    public Color outlineColour;
    public string[] tagList;
    public bool isUsing;
    public bool activatingSecondary;
    public float timeTillNewTarget = .5f;
    [SerializeField] float timeNo_timeTillNewTarget;
    [Header("Extra")]
    public GameObject extraGameObject;
    public Transform extraTransform;
    public Rigidbody2D rb;
    public float extraGOForce;
    [Header("Grabing")]
    [SerializeField] bool grabMode;
    public bool isUsing_Extra;
    [SerializeField] Vector2 currentMousePosition;
    [SerializeField] Vector2 oldMousePosition;
    public GameObject storedObject;
    [SerializeField] bool storedFlag;
    public Transform throwPoint;
    [SerializeField] bool throwFlag;
    public float useCooldown = 1f;
    public float timeNow_useCooldown;
    [Header("Fish")]
    public float minRange;
    [Header("Deply")]
    public GameObject deployObject;
    public Transform deployPosition;
    public int deployAmount;
    [SerializeField] List<GameObject> deployPool;
    [SerializeField] int deplyPoolPointer = -1;


    [Header("AI")]
    public bool AI = false;

    [Header("Sound")]
    public SoundManager soundManager;
    public Sound sound_Use1;
    public Sound sound_Use2;

    // Start is called before the first frame update
    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
        switch (weaponEnum)
        {
            case WeaponEnum.Deploy:
                //deployPool = new List<GameObject>(deployAmount);

                for (int i = 0; i < deployAmount; i++)
                {
                    deployPool.Add(Instantiate(deployObject, transform.position, Quaternion.identity));
                    deployPool[i].SetActive(false);
                }
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(transform.position);

        if (isUsing)
        {
            use();
        }
        else
        {
            stop();
        }

        highlightCurrentTarget();

        if (target == null && activatingSecondary)
        {
            stop();
        }


        if (timeNow_useCooldown > 0)
        {
            timeNow_useCooldown -= Time.deltaTime;
            switch (weaponEnum)
            {
                case WeaponEnum.Fish:
                    updateFishLine();
                    break;
            }
        }
        else
        {
            switch (weaponEnum)
            {
                case WeaponEnum.Fish:
                    extraGameObject.SetActive(false);
                    break;
            }
        }

        if ((target == null && storedObject != null))
        {

            //StartCoroutine(cooldownTillGrab());
            grabMode = false;
        }
        else if (!throwFlag)
        {
            grabMode = true;
        }

        if (activatingSecondary)
        {
            switch (weaponEnum)
            {
                case WeaponEnum.Hook:
                    if (target != null)
                    {
                        shootHook();

                    }
                    break;
                case WeaponEnum.Grab:
                    if (timeNow_useCooldown <= 0)
                    {
                        if (grabMode)
                        {
                            grabObject();
                        }
                        else
                        {
                            throwObject();
                        }
                    }
                    else
                    {

                    }

                    break;
                case WeaponEnum.Fish:
                    if (timeNow_useCooldown <= 0)
                    {
                        fishTarget();
                    }
                    else
                    {
                    }
                    break;
                case WeaponEnum.Deploy:
                    if (timeNow_useCooldown <= 0)
                    {
                        deployCover();
                    }
                    break;
            }
        }
        else
        {
            findTarget();
        }

        Debug.DrawRay(transform.position, (transform.up) * range, Color.green);

        /*
        if (target == null)
        //if (target == null && isUsing_Extra)
        {
            print("stoping");
            //isUsing_Extra = false;
            activatingSecondary = false;
            storedFlag = false;
            stop();
        }
        */
    }
    void highlightCurrentTarget()
    {
        if (target != (null))
        {
            try
            {
                //print(target.name + " update outline");
                target.GetComponent<OutlineSpriteScript>().setOutline(1f);
                target.GetComponent<OutlineSpriteScript>().setColour(outlineColour);
            }
            catch (System.Exception e)
            {
                print("ERROR: " + e.Message);
            }
            /*
            if (target.CompareTag("Pickup") || target.CompareTag("Object"))
            {
            }
            */
            clearTarget();
        }
    }

    void findTarget()
    {
        
        if (timeNow_useCooldown> 0)
        {
            return;
        }
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, transform.up, range, layerMask);

        if (hit)
        {
            if (tagList.Contains(hit.collider.tag))
            {
                target = hit.collider.gameObject;
                timeNo_timeTillNewTarget = timeTillNewTarget;
            }
        }
        else
        {
            if (timeNo_timeTillNewTarget <= 0)
            {
                target = null;

            }
            else
            {
                timeNo_timeTillNewTarget -= Time.deltaTime;
            }
        }
    }
    void clearTarget()
    {
        if ((target.transform.position - transform.position).magnitude > range)
        {
            target = null;
            //extraGameObject.SetActive(false);
        }
    }

    public void use()
    {
        if (timeNow_useCooldown <= 0)
        {
            activatingSecondary = true;

        }
        else
        {
            activatingSecondary = false;

        }
        /*
        switch (weaponEnum)
        {
            case WeaponEnum.Hook:
                if (timeNow_useCooldown <= 0)
                {
                    activatingSecondary = true;
                }

                break;
            case WeaponEnum.Grab:
                if (timeNow_useCooldown <= 0)
                {
                    activatingSecondary = true;

                }
                else
                {
                    activatingSecondary = false;

                }
                break;
        }
        */

        return;
    }
    public void stop()
    {

        switch (weaponEnum)
        {
            case WeaponEnum.Hook:
                activatingSecondary = false;

                if (extraGameObject.activeSelf)
                {
                    extraGameObject.SetActive(false);

                }
                if (rb != null)
                {
                    rb.velocity = new Vector2(0, 0);

                }
                break;
            case WeaponEnum.Grab:
                oldMousePosition = new Vector2(-3000, -3000);

                break;
            case WeaponEnum.Fish:
                if (extraGameObject.activeSelf)
                {
                    extraGameObject.SetActive(false);

                }
                oldMousePosition = new Vector2(-3000, -3000);

                break;
        }
    }


    public void toggleUse(InputAction.CallbackContext context)
    {
        isUsing = context.performed;
        //print("toggle Using: " + isUsing);

    }



    //Uses

    void shootHook()
    {
        //print("Hooking");
        //print((target.transform.position - transform.position).magnitude);

        findTarget();
        if (target == null || !target.activeSelf)
        {
            timeNow_useCooldown = useCooldown;
 
            stop();
            return;
        }
        if ((target.transform.position - transform.position).magnitude <= 1.5f)
        {

            timeNow_useCooldown = useCooldown;

            stop();
            return;
        }
        playSound_Use1();
        extraGameObject.SetActive(true);
        extraGameObject.transform.position = target.transform.position;
        Vector2 chainDir = (extraTransform.position - target.transform.position).normalized;
        extraGameObject.transform.rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(chainDir, Vector2.up), Vector3.forward);
        extraGameObject.transform.localScale = new Vector3(1, (target.transform.position - extraTransform.position).magnitude * transform.localScale.y * 2f, 1);
        Vector2 dir = (target.transform.position - extraTransform.position).normalized;
        rb.AddForce(dir * extraGOForce * GetComponentInParent<Rigidbody2D>().mass * Time.deltaTime);
    }



    void grabObject()
    {
        findTarget();
        currentMousePosition = Mouse.current.position.ReadValue() - new Vector2(Screen.width / 2, Screen.height / 2);
        if (target == null)
        {
            print("No target to grab");
            //print("stoping");
            //isUsing_Extra = false;
            activatingSecondary = false;
            storedFlag = false;

            stop();
            return;
        }
        else
        {

            print("Grabing " + target.name);

            if (checkGrab())
            {
                extraGameObject.GetComponent<VRHandMovementScript>().pickUp(Instantiate(target, target.transform.position, target.transform.rotation));
                target.SetActive(false);
                playSound_Use1();
                //rb = target.GetComponent<Rigidbody2D>();
                if (target.CompareTag("Pickup"))
                {
                    try
                    {
                        if (!target.GetComponent<PickupScript>().used)
                        {
                            GetComponent<WeaponTypeScript>().addAmmo(target.GetComponent<PickupScript>().amout);
                            target.GetComponent<PickupScript>().used = true;
                        }
                    }
                    catch (System.Exception e)
                    {
                        print("Error: " + e.Message);
                    }
                }
                else if (target.CompareTag("Object") && !storedFlag)
                {
                    storeObject();
                }
                activatingSecondary = false;
                throwFlag = false;

                //target = null;
                //rb.gravityScale = 1;
                //print("moving " + target.name + target.transform.position);
            }
            //activatingSecondary = false;
            //storedFlag = false;
            //storedObject = null;
            //timeNow_grabCooldown = grabCooldown;
        }

    }
    void fishTarget()
    {
        findTarget();
        currentMousePosition = Mouse.current.position.ReadValue();
        if (target == null)
        {
            print("No target to grab");
            //print("stoping");
            //isUsing_Extra = false;
            activatingSecondary = false;
            storedFlag = false;

            stop();
            return;
        }
        else
        {

            print("Grabing " + target.name);

            if (checkGrab())
            {
                //extraGameObject.GetComponent<VRHandMovementScript>().pickUp(Instantiate(target, target.transform.position, target.transform.rotation));
                //target.SetActive(false);
                playSound_Use1();
                fishPull();
                //rb = target.GetComponent<Rigidbody2D>();
                /*
                if (target.CompareTag("Pickup"))
                {
                    try
                    {
                        if (!target.GetComponent<PickupScript>().used)
                        {
                            GetComponent<WeaponTypeScript>().addAmmo(target.GetComponent<PickupScript>().amout);
                            target.GetComponent<PickupScript>().used = true;
                        }
                    }
                    catch (System.Exception e)
                    {
                        print("Error: " + e.Message);
                    }
                }
                else if (target.CompareTag("Object"))
                {
                    storeObject();
                }
                activatingSecondary = false;
                throwFlag = false;
                */

            }


        }

    }

    void fishPull()
    {
        playSound_Use1();
        extraGameObject.SetActive(true);
        print("Fishing "+target);
        //updateFishLine();
        Vector2 dir = (target.transform.position - extraTransform.position).normalized;
        try
        {
            rb = target.GetComponent<Rigidbody2D>();
            rb.AddForce(-dir * extraGOForce*rb.mass);
            //rb.velocity = (-dir * extraGOForce);
        }
        catch (System.Exception e)
        {
            Debug.LogError(target + " could not find rb to fish");
        }
    }

    void updateFishLine()
    {
        if (target == null)
        {
            activatingSecondary = false;
            extraGameObject.SetActive(false);
            return;
        }
        extraGameObject.transform.position = target.transform.position;
        Vector2 chainDir = (extraTransform.position - target.transform.position).normalized;
        extraGameObject.transform.rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(chainDir, Vector2.up), Vector3.forward);
        extraGameObject.transform.localScale = new Vector3(1, (target.transform.position - extraTransform.position).magnitude * transform.localScale.y * 2f, 1);

    }

    public bool checkGrab()
    {

        if (oldMousePosition.y <= 0f)
        {
            //print("updating old mouse");
            oldMousePosition = currentMousePosition;

        }

        float flick = currentMousePosition.y - oldMousePosition.y;
        if ((flick > 1) || AI)
        {
            //isUsing_Extra = true;
            timeNow_useCooldown = useCooldown;
            print("Grab Successful");
            activatingSecondary = false;

            return true;
        }
        else if (flick <= 0)
        {
            activatingSecondary = false;
            //isUsing_Extra = false;
            storedFlag = false;

        }
        print("Grab Failed");

        return false;
    }

    public void storeObject()
    {
        activatingSecondary = false;

        if (storedObject != null)
        {
            dropObject();
        }
        InteractableObjectScript interactableObjectScript;
        if (target.TryGetComponent<InteractableObjectScript>(out interactableObjectScript))
        {
            storedObject = target;
            target.GetComponent<BoxCollider2D>().enabled = false;

            storedFlag = true;
            print("storing: " + target.name);

        }
    }


    void resetStoreObject(Vector3 v, Quaternion q)
    {
        storedObject.transform.localScale = new Vector3(1, 1, 1);
        storedObject.transform.position = v;
        storedObject.transform.rotation = q;
        storedObject.GetComponent<BoxCollider2D>().enabled = true;
        storedObject.SetActive(true);
    }

    public void throwObject()
    {
        throwFlag = true;
        playSound_Use2();

        GameObject throwObject = storedObject;
        Vector3 newPoint = throwPoint.position + throwPoint.up * .2f;
        resetStoreObject(newPoint, throwPoint.rotation);
        throwObject.SetActive(true);
        InteractableObjectScript interactableObjectScript;
        if (throwObject.TryGetComponent<InteractableObjectScript>(out interactableObjectScript))
        {
            interactableObjectScript.YEET();
        }
        activatingSecondary = false;
        storedFlag = false;
        throwFlag = false;
        storedObject = null;
        timeNow_useCooldown = useCooldown;
        //Destroy(storedObject);
        //StartCoroutine(cooldownTillGrab());
    }

    public void dropObject()
    {
        print("droping: " + storedObject.name);
        //throwFlag = true;


        Vector3 newPoint = throwPoint.position + throwPoint.up * .5f;
        resetStoreObject(newPoint, throwPoint.rotation);
        storedObject = null;
        storedFlag = false;
        throwFlag = false;
        //GameObject throwObject = Instantiate(storedObject, newPoint, throwPoint.rotation);

        //Destroy(storedObject);
    }

    public void deployCover()
    {
        Vector3 newPoint = deployPosition.position + transform.rotation * ( (Vector3.up * range));
        GameObject currentDeploy = getNextDeploy();

        //Instantiate(deployObject, newPoint, Quaternion.identity);
        //print("deploy at: " + newPoint);
        currentDeploy.SetActive(true);
        currentDeploy.transform.position = newPoint;
        DestructableScript d;
        if (currentDeploy.TryGetComponent<DestructableScript>(out d))
        {
            currentDeploy.GetComponent<DestructableScript>().Rewind();

        }


        timeNow_useCooldown = useCooldown;
        playSound_Use1();
    }

    GameObject getNextDeploy()
    {
        foreach (GameObject g in deployPool)
        {
            if (!g.gameObject.activeSelf)
            {
                return g;
            }
        }

        deplyPoolPointer = (deplyPoolPointer + 1) % deployAmount;
        return deployPool[deplyPoolPointer];

    }

    public void Rewind()
    {
        if (storedObject != null)
        {
            dropObject();
        }
        switch (weaponEnum)
        {
            case WeaponEnum.Deploy:
                //deployPool = new List<GameObject>(deployAmount);

                foreach(GameObject g in deployPool)
                {
                    //deployPool.Add(Instantiate(deployObject, transform.position, Quaternion.identity));
                    g.SetActive(false);
                }
                break;
        }
    }



    IEnumerator cooldownTillGrab()
    {
        yield return new WaitForSeconds(useCooldown);
        activatingSecondary = false;
        storedFlag = false;
        throwFlag = false;
        print("finish cooldown");

    }


    //Sound



    void playSound_Use1()
    {
        soundManager.Play(sound_Use1.name);

    }
    void playSound_Use2()
    {
        soundManager.Play(sound_Use2.name);

    }

}
