﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseSecondaryScript : MonoBehaviour
{
    [SerializeField] private WeaponEnum weaponEnum;
    public GameObject target;
    public float range;
    [SerializeField] LayerMask layerMask;
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
    public float grabCooldown = 1f;
    [SerializeField] float timeNow_grabCooldown;

    // Start is called before the first frame update
    void Awake()
    {

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

        if (target != (null))
        {
            if (target.CompareTag("Pickup") || target.CompareTag("Object"))
            {
                try
                {
                    target.GetComponent<InteractableObjectScript>().setOutline(1f);
                }
                catch (System.Exception e)
                {
                    print("ERROR: " + e.Message);
                }
            }
            clearTarget();
        }
        if (target == null && activatingSecondary)
        {
            stop();
        }


        if (timeNow_grabCooldown > 0)
        {
            timeNow_grabCooldown -= Time.deltaTime;
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
                    if (timeNow_grabCooldown <= 0)
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
            }
        }
        else
        {
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

        //Debug.DrawRay(transform.position, (transform.right) *range, Color.green);

        if (target == null && isUsing_Extra)
        {
            print("stoping");
            isUsing_Extra = false;
            activatingSecondary = false;
            storedFlag = false;
            stop();
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
        activatingSecondary = true;
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
                //print("old mouse reset");
                oldMousePosition = new Vector2(-3000, -3000);

                break;
        }
    }


    public void toggleUse(InputAction.CallbackContext context)
    {
        isUsing = context.performed;
        print("toggle Using: " + isUsing);

    }

    void shootHook()
    {
        print((target.transform.position - transform.position).magnitude);
        if ((target.transform.position - transform.position).magnitude <= 1.5f)
        {

            stop();
            return;
        }
        extraGameObject.SetActive(true);
        extraGameObject.transform.position = target.transform.position;
        Vector2 chainDir = (extraTransform.position - target.transform.position).normalized;
        extraGameObject.transform.rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(chainDir, Vector2.up), Vector3.forward);
        extraGameObject.transform.localScale = new Vector3(1, (target.transform.position - extraTransform.position).magnitude * transform.localScale.y * 2f, 1);
        Vector2 dir = (target.transform.position - extraTransform.position).normalized;
        rb.AddForce(dir * extraGOForce * Time.deltaTime);
    }



    void grabObject()
    {
        currentMousePosition = Mouse.current.position.ReadValue();
        if (target == null)
        {
            //print("stoping");
            isUsing_Extra = false;
            activatingSecondary = false;
            storedFlag = false;

            stop();
            //return;
        }
        else
        {

            print("Grabing " + target.name);

            if (oldMousePosition.y <= 0f)
            {
                //print("updating old mouse");
                oldMousePosition = currentMousePosition;

            }

            float flick = currentMousePosition.y - oldMousePosition.y;
            if (flick > 1 && !isUsing_Extra)
            {
                isUsing_Extra = true;
                extraGameObject.GetComponent<VRHandMovementScript>().pickUp(target);

                //target.transform.SetParent(extraGameObject.transform);
                //Destroy(target, timeToDisappear);
            }
            else if (flick <= 0)
            {
                activatingSecondary = false;
                isUsing_Extra = false;
                storedFlag = false;

            }

            if (isUsing_Extra)
            {
                //rb = target.GetComponent<Rigidbody2D>();
                if (target.CompareTag("Pickup"))
                {
                    try
                    {
                        if (!target.GetComponent<PickupScript>().used)
                        {
                            GetComponent<WeaponTypeScript>().Ammo += target.GetComponent<PickupScript>().amout;
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
                    if (storedObject != null)
                    {
                        dropObject();
                    }
                    InteractableObjectScript interactableObjectScript;
                    if (target.TryGetComponent<InteractableObjectScript>(out interactableObjectScript))
                    {
                        storedObject = Instantiate(interactableObjectScript.prefab);
                        storedObject.transform.localScale = new Vector3(1, 1, 1);
                        storedObject.GetComponent<BoxCollider2D>().enabled = true;
                        storedObject.SetActive(false);
                        storedFlag = true;
                        print("storing: " + target.name);

                    }
                }
                target.GetComponent<BoxCollider2D>().enabled = false;

                //rb.gravityScale = 1;
                //print("moving " + target.name + target.transform.position);
            }
            timeNow_grabCooldown = grabCooldown;
        }

    }

    public void throwObject()
    {
        throwFlag = true;
        storedObject.GetComponent<BoxCollider2D>().enabled = true;
        storedObject.SetActive(true);

        Vector3 newPoint = throwPoint.position + throwPoint.up * 1f;
        GameObject throwObject = Instantiate(storedObject, newPoint, throwPoint.rotation);
        InteractableObjectScript interactableObjectScript;
        if (throwObject.TryGetComponent<InteractableObjectScript>(out interactableObjectScript))
        {
            interactableObjectScript.YEET();
        }
        activatingSecondary = false;
        storedFlag = false;
        throwFlag = false;
        Destroy(storedObject);
        //StartCoroutine(cooldownTillGrab());
    }

    public void dropObject()
    {
        print("droping: " + storedObject.name);
        //throwFlag = true;
        storedObject.GetComponent<BoxCollider2D>().enabled = true;
        storedObject.SetActive(true);

        Vector3 newPoint = throwPoint.position + throwPoint.up * 1.2f;
        storedObject.transform.position = newPoint;
        storedObject.transform.rotation = Quaternion.identity;
        storedObject = null;
        activatingSecondary = false;
        storedFlag = false;
        throwFlag = false;
        //GameObject throwObject = Instantiate(storedObject, newPoint, throwPoint.rotation);

        //Destroy(storedObject);
    }



    IEnumerator cooldownTillGrab()
    {
        yield return new WaitForSeconds(grabCooldown);
        activatingSecondary = false;
        storedFlag = false;
        throwFlag = false;
        print("finish cooldown");

    }


}
