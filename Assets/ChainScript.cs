using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainScript : MonoBehaviour
{
    public Renderer[] renderers;
    [SerializeField] private float chainSize;
    [SerializeField] private float chainSpeed;


    private void Start()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material.SetFloat("_ChainSpeed", chainSpeed);

        }
    }


    // Update is called once per frame
    void Update()
    {
            setChain();

    }

    private void setChain()
    {
        chainSize = transform.localScale.y;
        foreach (Renderer renderer in renderers)
        {
            renderer.material.SetFloat("_ChainAmount", chainSize);

        }

    }

    private void setChainSpeed(float f)
    {
        chainSpeed = f;
        foreach (Renderer renderer in renderers)
        {
            renderer.material.SetFloat("_ChainSpeed", chainSpeed * f);

        }


    }

}
