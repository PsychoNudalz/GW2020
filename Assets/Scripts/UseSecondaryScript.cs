using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UseSecondaryScript : MonoBehaviour
{
    [SerializeField] private WeaponEnum weaponEnum;
    public GameObject target;
    public float range;
    [SerializeField] LayerMask layerMask;
    public string[] tagList;
    public bool isUsing;
    [Header("Extra")]
    public bool isUsing_Extra;
    public GameObject extraGameObject;
    public Transform extraTransform;
    public Rigidbody2D rb;
    public float extraGOForce;
    Vector2 oldMousePosition;
    public float timeToDisappear;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //print(transform.position);

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
                    print("ERROR");
                }
            }
            clearTarget();
        }
        if (target == null)
        {
            stop();
        }

        if (isUsing)
        {
            switch (weaponEnum)
            {
                case WeaponEnum.Hook:
                    shootHook();
                    break;
                case WeaponEnum.Grab:
                    grabObject();
                    break;
            }
        }


        RaycastHit2D hit;
        if (!isUsing)
        {
            hit = Physics2D.Raycast(transform.position, transform.up, range, layerMask);

            if (hit)
            {
                if (tagList.Contains(hit.collider.tag))
                {
                    target = hit.collider.gameObject;
                }
            }
        }

        //Debug.DrawRay(transform.position, (transform.right) *range, Color.green);
    }

    void clearTarget()
    {
        if ((target.transform.position - transform.position).magnitude > range)
        {
            target = null;
            //extraGameObject.SetActive(false);
        }
    }

    public bool use()
    {
        if (target != (null))
        {
            isUsing = true;

            return true;


        }
        return false;
    }
    public void stop()
    {

        switch (weaponEnum)
        {
            case WeaponEnum.Hook:
                isUsing = false;
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
                //oldMousePosition.y = 0;

                break;
        }
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
        if (target == null)
        {
            //print("stoping");
            isUsing_Extra = false;
            isUsing = false;
            stop();
            return;
        }
        //print("Grabing " + target.name);

        if (oldMousePosition.y == 0f)
        {
            oldMousePosition = Input.mousePosition;

            return;
        }
        float flick = Input.mousePosition.y - oldMousePosition.y;
        if (flick > 0 && !isUsing_Extra)
        {
            isUsing_Extra = true;
            extraGameObject.GetComponent<VRHandMovementScript>().pickUp(target);
            //target.transform.SetParent(extraGameObject.transform);
            //Destroy(target, timeToDisappear);
        }
        else if (flick < 0)
        {
            isUsing = false;
            isUsing_Extra = false;

            return;
        }

        if (isUsing_Extra)
        {
            target.GetComponent<BoxCollider2D>().enabled = false;
            rb = target.GetComponent<Rigidbody2D>();
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

                }
            }
            //rb.gravityScale = 1;
            //print("moving " + target.name + target.transform.position);
        }
    }


}
