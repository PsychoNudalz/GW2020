using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandMovementScript : MonoBehaviour
{
    [Header("Left Hand")]
    public Transform leftHand;
    public Animator leftHandAnimator;
    [SerializeField] Vector3 left_INITIAL;
    public Transform leftPickup;
    public Transform leftDropoff;
    [SerializeField] bool pickingUp;
    [SerializeField] bool phase1 = false;
    [SerializeField] bool phase2 = false;



    [Header("Right Hand")]
    public Transform rightHand;
    public Animator rightHandAnimator;
    [SerializeField] Vector3 right_INITIAL;

    [Header("Mouse Movement")]
    [SerializeField] Vector3 mousePosition = new Vector3();
    public float midScale = 0.2f;
    public float maxDistance;


    // Start is called before the first frame update
    void Start()
    {
        left_INITIAL = leftHand.localPosition;
        right_INITIAL = rightHand.localPosition;
        leftHand.position = left_INITIAL;
        rightHand.position = right_INITIAL;
    }

    // Update is called once per frame
    void Update()
    {
        setHandPosition();

        if (pickingUp)
        {
            leftHandPickUp();
        }
    }
    void setHandPosition()
    {
        mousePosition = (Input.mousePosition);
        Vector3 displace = ((mousePosition - transform.position - new Vector3(Screen.width / 2, Screen.height / 2)) * midScale);
        if (displace.magnitude > maxDistance)
        {
            displace = displace.normalized * maxDistance;
        }
        transform.localPosition = new Vector3(displace.x,displace.y,5);
        //transform.localPosition.z = 0f;
        //print(mousePosition + ", " + transform.position + ", " + midpoint.transform.position + ", " + new Vector3(Screen.width / 2, Screen.height / 2));
    }

    void leftHandPickUp()
    {
        
        print((leftPickup.position - leftHand.position).magnitude+"  "+ leftPickup.position + "  " + leftHand.position);
        if (!pickingUp)
        {

            pickingUp = true;
        }
        if ((leftPickup.position-leftHand.position).magnitude > 2f && !phase1)
        {
            leftHand.position = Vector2.Lerp(leftHand.position, leftPickup.position,Time.deltaTime*1.5f);
            print("moving to Pick");
            leftHandAnimator.SetBool("PickUp", true);

        } else if ((leftDropoff.position - leftHand.position).magnitude > 7f &&!phase2)
        {
            print("moving to Drop");
            phase1 = true;
            leftHand.position = Vector2.Lerp(leftHand.position, leftDropoff.position,Time.deltaTime*3f);
        }
        else if ((left_INITIAL - leftHand.position).magnitude > 5f)
        {
            leftHand.position = Vector2.Lerp(leftHand.position, left_INITIAL, Time.deltaTime * 3f); ;
            leftHandAnimator.SetBool("PickUp", false);
            phase2 = true;
        }
        else
        {
            leftHand.localPosition = left_INITIAL;
            pickingUp = false;
            phase1 = false;
            phase2 = false;

        }
    }
    public void pickUp()
    {
        phase1 = false;
        phase2 = false;
        pickingUp = true;
        leftHandAnimator.SetBool("PickUp", false);
        leftHandAnimator.SetBool("PickUp", true);


    }
}
