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
    public float duration = 0;
    public float timeStart;
    public Vector3 characterLocation;

    public EventType(Vector2 d, Vector2 v)
    {
        timeStart = Time.time;
        moveDir = d;
        mouseLocation = v;
        logs = new List<LogType>();


    }

    public void addLog(string s)
    {
        logs.Add(new LogType(s));
    }

    public void endLog(Vector3 loc)
    {
        duration = Time.time - timeStart;
        characterLocation = loc;
        
        if (logs == null)
        {
            logs = new List<LogType>();
            addLog("Null");

        }
        
    }

    public override string ToString()
    {
        return ("Logs: " + logs.Count + "    Duration: " + duration+"\n"+ moveDir + "\n");
    }

}
