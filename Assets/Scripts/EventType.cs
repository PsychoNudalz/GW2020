using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventType
{
    public List<LogType> logs = new List<LogType>();
    public Vector2 moveDir;
    public Vector2 mouseLocation;
    public float duration = -1;
    public float timeStart;

    public EventType(Vector2 v)
    {
        timeStart = Time.time;
        mouseLocation = v;
        
    }

    public void addLog(string s)
    {
        logs.Add(new LogType(s));
    }

    public void endLog()
    {
        duration = Time.time - duration;
    }

    public override string ToString()
    {
        return ("Logs: " + logs.Count + " Duration: " + duration);
    }

}
