using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class VRHandMovementScript : MonoBehaviour
{
    [Header("Left Hand")]
    public Transform leftHand;
    public Animator leftHandAnimator;
    [SerializeField] Vector3 left_INITIAL;
    public Transform leftPickup;
    public Transform leftDropoff;
    public GameObject pickedUpObject;
    [SerializeField] Transform pickedUpObject_Parent;
    [SerializeField] float scaleUpRate;

    [SerializeField] bool pickingUp;
    [SerializeField] bool phase1 = false;
    [SerializeField] bool phase2 = false;



    [Header("Right Hand")]
    public Transform rightHand;
    public Animator rightHandAnimator;
    [SerializeField] Vector3 right_INITIAL;
    public Transform reloadPoint;
    public WeaponTypeScript weapon;

    [Header("Mouse Movement")]
    [SerializeField] Transform midPointPosition;
    public GameObject followTarget;
    public float midScale = 0.2f;
    public float maxDistance;


    // Start is called before the first frame update
    void Start()
    {
        //left_INITIAL = leftHand.localPosition;
        //right_INITIAL = rightHand.localPosition;
        //leftHand.position = left_INITIAL;
        //rightHand.position = right_INITIAL;
        leftHand.position = left_INITIAL + transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //print(transform.position);
        //print(transform.localPosition);

        //setHandPosition();

        if (pickingUp)
        {
            leftHandPickUp();
        }
        else
        {
            leftHandHelpReload();
        }
    }
    void setHandPosition()
    {

        Vector3 displace = (midPointPosition.position);
        if (displace.magnitude > maxDistance)
        {
            displace = displace.normalized * maxDistance;
        }
        transform.position = new Vector3(displace.x + followTarget.transform.position.x, displace.y + followTarget.transform.position.y, 0);
        //transform.localPosition.z = 0f;
        //print(mousePosition + ", " + transform.position + ", " + midpoint.transform.position + ", " + new Vector3(Screen.width / 2, Screen.height / 2));
    }

    void leftHandPickUp()
    {

        //print((leftPickup.position - leftHand.position).magnitude+"  "+ leftPickup.position + "  " + leftHand.position);
        if (!pickingUp)
        {

            pickingUp = true;
        }
        if (pickedUpObject.activeSelf && pickedUpObject != null)
        {
            if (pickedUpObject.transform.lossyScale.x < 3f && !phase1 && pickedUpObject != null)
            {

                pickedUpObject.transform.localScale += pickedUpObject.transform.localScale * scaleUpRate * Time.deltaTime;
                pickedUpObject.transform.position = Vector2.Lerp(pickedUpObject.transform.position, leftHand.position, Time.deltaTime * 1.5f);

                leftHand.position = Vector2.Lerp(leftHand.position, leftPickup.position, Time.deltaTime * 1.5f);

                //print("moving to Pick: " + (leftPickup.position - leftHand.position).magnitude);

            }
            else if ((leftDropoff.position - leftHand.position).magnitude > .7f && !phase2 && pickedUpObject != null)
            {
                leftHandAnimator.SetBool("PickUp", true);
                //print("moving to Drop: " + (leftDropoff.position - leftHand.position).magnitude);
                phase1 = true;
                leftHand.position = Vector2.Lerp(leftHand.position, leftDropoff.position, Time.deltaTime * 3f);
            }
            else if ((left_INITIAL + transform.position - leftHand.position).magnitude > .5f)
            {
                pickedUpObject.SetActive(false);
                //Destroy(pickedUpObject,2f);
            }
        }
        else if ((left_INITIAL + transform.position - leftHand.position).magnitude > .5f)
        {
            //Destroy(pickedUpObject);

            leftHand.position = Vector2.Lerp(leftHand.position, left_INITIAL + transform.position, Time.deltaTime * 3f); ;
            leftHandAnimator.SetBool("PickUp", false);
            phase2 = true;

        }
        else
        {
            leftHand.position = left_INITIAL + transform.position;
            pickingUp = false;
            phase1 = false;
            phase2 = false;

        }
    }
    public void pickUp(GameObject g)
    {
        clearLeftHand();
        phase1 = false;
        phase2 = false;
        pickingUp = true;
        leftHandAnimator.SetBool("PickUp", false);
        //leftHandAnimator.SetBool("PickUp", true);
        pickedUpObject = g;
        pickedUpObject.GetComponent<BoxCollider2D>().enabled = false;
        //pickedUpObject_Parent = pickedUpObject.transform.parent;
        g.transform.SetParent(leftHand);


    }

    void leftHandHelpReload()
    {
        if (weapon.isReloading)
        {
            //print("reloading");
            leftHandAnimator.SetTrigger("Reload");
            leftHand.position = Vector2.Lerp(leftHand.position, reloadPoint.position, Time.deltaTime * 2f);


        }
        else
        {
            leftHandAnimator.SetTrigger("FinishReload");

            leftHand.position = Vector2.Lerp(leftHand.position, left_INITIAL + transform.position, Time.deltaTime * 3f); ;
            //leftHand.position = left_INITIAL + transform.position;

        }
    }

    void clearLeftHand()
    {
        for (int i = 0; i < leftHand.childCount; i++)
        {
            //leftHand.GetChild(i).gameObject.transform.SetParent(pickedUpObject_Parent);
            Destroy(leftHand.GetChild(i).gameObject,1f);
        }
    }
}
