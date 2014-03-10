using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class ConfigManager : MonoBehaviour
{
    public PlayerConfig[] PlayerConfigs = new [] {
        new PlayerConfig { ControlType = ControlType.Joystick1 },
        new PlayerConfig { ControlType = ControlType.Joystick2 },
        new PlayerConfig { ControlType = ControlType.Joystick3 },
        new PlayerConfig { ControlType = ControlType.Joystick4 },
    };

    public MapGenerationOptions MapGenerationOptions = new MapGenerationOptions {
        Width = 24,
        Height = 100
    };

    void Awake()
    {
        for( int i = 0; i < PlayerConfigs.Length; i++ )
        {
            PlayerConfigs[i].PlayerNumber = i;
        }
    }
}
