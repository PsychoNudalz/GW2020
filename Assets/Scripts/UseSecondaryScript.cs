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
        isUsing = false;
        if (extraGameObject.activeSelf)
        {
            extraGameObject.SetActive(false);

        }
        rb.velocity = new Vector2(0, 0);
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
}
