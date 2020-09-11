using System;
using System.Collections.Generic;

[Serializable]
public class LogType : IEquatable<LogType>
{

    //[SerializeField] private InputEnum inputEnum;
    public string inputType;

    public LogType(string s)
    {
        inputType = s;
        /*
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
        else if (s.Equals("Move"))
        {
            inputEnum = InputEnum.Move;
        }
        else if (s.Equals("Shoot"))
        {
            inputEnum = InputEnum.Shoot;

        }
        else if (s.Equals("Use"))
        {
            inputEnum = InputEnum.Use;

        } else if (s.Equals("Reload"))
        {
            inputEnum = InputEnum.Reload;
        }
        else
        {
            inputEnum = InputEnum.NA;
        }
        */
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj as LogType);
    }
    public bool Equals(String input)
    {

        return inputType.Equals(input);
    }

    public bool Equals(LogType other)
    {
        return other != null &&
               inputType == other.inputType;
    }

    public override int GetHashCode()
    {
        return 1020398523 + EqualityComparer<string>.Default.GetHashCode(inputType);
    }
}
