using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    [Header("Movement")]
    public Rigidbody2D rb;
    public float moveSpeed = 10;
    [SerializeField] Vector3 playerInput = new Vector3();
    [Header("Camera")]
    [SerializeField] Vector3 mousePosition = new Vector3();
    public GameObject midpoint;
    public float midScale = 0.2f;
    public float maxDistance;
    [Header("Aiming Weapon")]
    public GameObject gun;
    [Header("Animator")]
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerControls();
        updateAnimation();
        setMidPosition();
        aimWeapon();
    }

    void playerControls()
    {
        /*
        if (Input.GetAxis("Horizontal") > 0)
        {
            playerInput += new Vector3(1, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            playerInput += new Vector3(-1, 0);
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            playerInput += new Vector3(0, 1);
        }
        else if (Input.GetAxis("Vertical") < 0)

        {
            playerInput += new Vector3(0, -1);
            print(Input.GetAxis("Horizontal"));

        }

        playerInput = new Vector3(math.floor(Input.GetAxis("Horizontal")), (int)(Input.GetAxis("Vertical") + 0.1f));
        //print(playerInput);
        transform.position += playerInput.normalized * moveSpeed * Time.deltaTime;
        */
        /*
        playerInput = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            playerInput.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerInput.y = -1;

        }
        if (Input.GetKey(KeyCode.D))
        {
            playerInput.x = 1;

        }
        else if (Input.GetKey(KeyCode.A))

        {
            playerInput.x = -1;

        }
        */
        playerInput.x = Input.GetAxisRaw("Horizontal");
        playerInput.y = Input.GetAxisRaw("Vertical");
        transform.position += playerInput.normalized * moveSpeed * Time.deltaTime;

    }

    void setMidPosition()
    {
        mousePosition = (Input.mousePosition);
        Vector3 displace = ((mousePosition - transform.position - new Vector3(Screen.width / 2, Screen.height / 2)) * midScale);
        if (displace.magnitude > maxDistance)
        {
            displace = displace.normalized * maxDistance;
        }
        midpoint.transform.position = transform.position + displace;
        //print(mousePosition + ", " + transform.position + ", " + midpoint.transform.position + ", " + new Vector3(Screen.width / 2, Screen.height / 2));
    }

    void aimWeapon()
    {
        //gun.transform.LookAt((Vector2)midpoint.transform.position);

        UnityEngine.Vector2 dir = midpoint.transform.position - transform.position;
        dir.Normalize();
        //print(gun.transform.rotation.eulerAngles.z);
        Vector3 originalScale = gun.transform.localScale;
        gun.transform.rotation = Quaternion.AngleAxis(-Vector2.SignedAngle(dir, Vector2.up), transform.forward);
        if (gun.transform.rotation.eulerAngles.z < 180f && gun.transform.rotation.eulerAngles.z > 0)
        {
            originalScale.x = 1;

        }
        else
        {
            originalScale.x = -1;

        }
        gun.transform.localScale = originalScale;
    }

    private void updateAnimation()
    {
        animator.SetFloat("Speed", playerInput.normalized.magnitude);
        //print(animator.GetFloat("Speed"));
    }

}
