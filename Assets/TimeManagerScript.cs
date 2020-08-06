using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    public void slowDown(float factor, float length)
    {
        slowdownFactor = factor;
        slowdownLength = length;
        Time.timeScale = factor;
        //Time.fixedDeltaTime = Time.timeScale * .2f;
        StartCoroutine(waitTillReset(length*factor));
    }

    public void resetTime()
    {
        print("reseting");
        Time.timeScale = 1;
        //Time.fixedDeltaTime = Time.timeScale * .2f;
    }

    IEnumerator waitTillReset(float t)
    {
        yield return new WaitForSeconds(t);
        resetTime();
    }



}
