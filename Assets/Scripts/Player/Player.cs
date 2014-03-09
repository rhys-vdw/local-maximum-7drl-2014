using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    PlayerConfig m_Config;
    public event Action<PlayerConfig> ConfigureEvent;

    public void Configure( PlayerConfig config )
    {
        m_Config = config;
        if( ConfigureEvent != null ) ConfigureEvent( config );
    }
}
