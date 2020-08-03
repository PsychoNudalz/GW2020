using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript : MonoBehaviour
{
    public Renderer renderer;
    public float decayValue;
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
        print(renderer.material.GetFloat("_Outline"));
    }

    public void setOutline(float f)
    {
        renderer.material.SetFloat("_Outline", f);
    }
}
