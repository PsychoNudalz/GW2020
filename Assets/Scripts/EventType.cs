using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [Serializable]
public class EventType
{
    public List<LogType> logs;
    public Vector2 moveDir;
    public Vector2 mouseLocation;
    //public float duration = 0;
    public float timeStart;
    public float timeEnd;
    public Vector3 characterLocation;
    public bool weaponFire = false;
    public Quaternion fireDirection;

    public EventType(Vector2 d, Vector2 v,float t)
    {
        timeStart = t;
        timeEnd = t;
        moveDir = d;
        mouseLocation = v;
        logs = new List<LogType>();
    }

    public EventType(EventType e)
    {
        logs = e.logs;
        moveDir = e.moveDir;
        mouseLocation = e.mouseLocation;
        timeStart = e.timeStart;
        timeEnd = e.timeEnd;
        characterLocation = e.characterLocation;
        weaponFire = e.weaponFire;
        fireDirection = e.fireDirection;
    }

    public void addLog(string s)
    {
        logs.Add(new LogType(s));
    }

    public void endLog(Vector3 loc,float t)
    {
        //duration = Time.time - timeStart;
        timeEnd = t;
        characterLocation = loc;
        
        if (logs == null)
        {
            logs = new List<LogType>();
            addLog("Null");

        }
        
    }
    public void setFiredToTrue(Quaternion dir)
    {
        weaponFire = true;
        fireDirection = dir;
    }

    public override string ToString()
    {
        return ("Logs: " + logs.Count + "    Time End: " + timeEnd+"\n"+ moveDir + "\n");
    }

}
