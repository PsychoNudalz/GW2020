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
    public GameObject extraGameObject;
    public Transform extraTransform;
    public Rigidbody2D rb;
    public float extraGOForce;
    Vector2 oldMousePosition;
    [SerializeField] float scaleUpRate;
    public float timeToDisappear;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (target != (null))
        {
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
            extraGameObject.SetActive(false);
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
        print("Grabing " + target.name);
        if (oldMousePosition.y == 0f)
        {
            oldMousePosition = Input.mousePosition;
            return;
        }
        float flick = Input.mousePosition.y - oldMousePosition.y;
        if (flick <= 0)
        {
            //target.GetComponent<BoxCollider2D>().enabled = true;

            stop();
            return;
        }
        target.GetComponent<BoxCollider2D>().enabled = false;
        rb = target.GetComponent<Rigidbody2D>();
        //rb.gravityScale = 1;
        target.transform.position = Vector2.Lerp(target.transform.position, extraGameObject.transform.position, Time.deltaTime);
        Destroy(target, 5f);
    }
}
