using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class ConfigManager : MonoBehaviour
{
    public ControlType[] ControlTypes = new ControlType[] {
        ControlType.Joystick1,
        ControlType.Joystick2,
        ControlType.Joystick3,
        ControlType.Joystick4,
    };

    public MapGenerationOptions MapGenerationOptions = new MapGenerationOptions {
        Width = 24,
        Height = 100
    };
}
