using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineSpriteScript : MonoBehaviour
{
    public SpriteRenderer renderer;
    public float decayValue = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (renderer == null)
        {
            renderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (renderer.material.GetFloat("_Outline") > 0f)
        {
            renderer.material.SetFloat("_Outline", renderer.material.GetFloat("_Outline") - Time.deltaTime * decayValue);
        }
    }
    public void setOutline(float f)
    {
        renderer.material.SetFloat("_Outline", f);
    }
    public void setColour(Color c)
    {
        renderer.material.SetColor("_OutlineColour", c);
    }

}
