using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{

    [Header("Movement")]
    public Rigidbody2D rb;
    public float moveSpeed = 10;
    public bool isPlayer;
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
        
        //playerInput.x = Input.GetAxisRaw("Horizontal");
        //playerInput.y = Input.GetAxisRaw("Vertical");
        //rb.velocity = playerInput.normalized * moveSpeed ;
        transform.position += playerInput.normalized * moveSpeed*Time.deltaTime;
    }

    public void inputPlayerControls(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }


    public void playerControls(Vector2 v)
    {

        playerInput = v;
        //rb.velocity = playerInput.normalized * moveSpeed ;
        //transform.position += playerInput.normalized * moveSpeed * Time.deltaTime;
    }

    public void setMidPosition()
    {
        mousePosition = Mouse.current.position.ReadValue();
        Vector3 displace = ((mousePosition - transform.position - new Vector3(Screen.width / 2, Screen.height / 2)) * midScale);
        if (displace.magnitude > maxDistance)
        {
            displace = displace.normalized * maxDistance;
        }
        midpoint.transform.position = transform.position + displace;
        //print(mousePosition + ", " + transform.position + ", " + midpoint.transform.position + ", " + new Vector3(Screen.width / 2, Screen.height / 2));
    }

    public void aimWeapon()
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

    public void setMousePosition(Vector3 v)
    {
        mousePosition = v;
    }

    private void updateAnimation()
    {
        animator.SetFloat("Speed", playerInput.normalized.magnitude);
        //print(animator.GetFloat("Speed"));
    }

}
