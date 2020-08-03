using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogType
{

    [SerializeField] private InputEnum inputEnum;

    public LogType(string s)
    {
        if (s.Equals("Up"))
        {
            inputEnum = InputEnum.Up;
        }
        else if (s.Equals("Down"))
        {
            inputEnum = InputEnum.Down;

        }
        else if (s.Equals("Left"))
        {
            inputEnum = InputEnum.Left;

        }
        else if (s.Equals("Right"))
        {
            inputEnum = InputEnum.Right;

        }
        else if (s.Equals("Shoot"))
        {
            inputEnum = InputEnum.Shoot;

        }
        else if (s.Equals("Use"))
        {
            inputEnum = InputEnum.Use;

        }
    }


}
