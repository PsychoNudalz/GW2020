using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    public float currentTime = 0;
    public float startTime = 0;


    public void Awake()
    {
        Time.timeScale = 1f;
    }
    private void FixedUpdate()
    {
        resetWorldTime();
    }

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

    public void resetWorldTime()
    {
        currentTime = Time.time - startTime;
    }

    public void setStartTime()
    {
        startTime = Time.time;
    }

    IEnumerator waitTillReset(float t)
    {
        yield return new WaitForSeconds(t);
        resetTime();
    }

    public string getCurrentTimeDisplay()
    {
        return currentTime.ToString();
    }

}
