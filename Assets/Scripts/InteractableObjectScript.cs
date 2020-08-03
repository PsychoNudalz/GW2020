using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript : MonoBehaviour
{
    public SpriteRenderer renderer;
    public float decayValue = 1f;
    public GameObject prefab;
    [Header("YEEEET! values")]
    public Rigidbody2D rb;
    public float damage;
    public float force;
    public float spin;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (renderer.material.GetFloat("_Outline") > 0f)
        {
            renderer.material.SetFloat("_Outline", renderer.material.GetFloat("_Outline") - Time.deltaTime * decayValue);
        }
        //print(renderer.material.GetFloat("_Outline"));
    }

    public void setOutline(float f)
    {
        renderer.material.SetFloat("_Outline", f);
    }

    public void YEET()
    {
        rb.AddForce(transform.up * force);
        rb.AddTorque(spin);
        print(name + " Yeet ");
    }
}
