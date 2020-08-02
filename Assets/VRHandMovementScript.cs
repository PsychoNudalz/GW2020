using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandMovementScript : MonoBehaviour
{
    [Header("Left Hand")]
    public Transform leftHand;
    public Animator leftHandAnimator;
    [Header("Right Hand")]
    public Transform rightHand;
    public Animator rightHandAnimator;
    [Header("Mouse Movement")]
    [SerializeField] Vector3 mousePosition = new Vector3();
    public float midScale = 0.2f;
    public float maxDistance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setHandPosition();
    }
    void setHandPosition()
    {
        mousePosition = (Input.mousePosition);
        Vector3 displace = ((mousePosition - transform.position - new Vector3(Screen.width / 2, Screen.height / 2)) * midScale);
        if (displace.magnitude > maxDistance)
        {
            displace = displace.normalized * maxDistance;
        }
        transform.position = displace;
        //print(mousePosition + ", " + transform.position + ", " + midpoint.transform.position + ", " + new Vector3(Screen.width / 2, Screen.height / 2));
    }
}
