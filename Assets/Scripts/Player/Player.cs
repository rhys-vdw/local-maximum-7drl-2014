using UnityEngine;
using System.Collections;
using System;
using EventTools;

public class Player : MonoBehaviour
{
    public Watchable<PlayerConfig> Config;

    public void Configure( PlayerConfig config )
    {
        Config.Value = config;
    }
}
