using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityObjectRetrieval;

public class JoystickInputMapping : ExtendedMonoBehaviour, IInputMapping
{
    // Constants.

    enum Button
    {
        A = 0,
        B = 1,
        X = 2,
        Y = 3,
        LeftBumper = 4,
        RightBumper = 5,
        Back = 6,
        Start = 7,
        LeftJoystick = 8,
        RightJoystick = 9
    }

    enum Axis
    {
        LeftX = 0,
        LeftY = 1,
        BothTriggers = 2,
        RightX = 3,
        RightY = 4,
        DPadX = 5,
        DPadY = 6,
        LeftTrigger = 8,
        RightTrigger = 9
    }

    enum KeyState
    {
        None,
        Down,
        Held,
        Up
    }

    static readonly Dictionary<PlayerKey, Button> KeyMapping = new Dictionary<PlayerKey, Button>() {
        { PlayerKey.SwapLeft, Button.LeftBumper },
        { PlayerKey.SwapRight, Button.RightBumper },
        { PlayerKey.PickUp, Button.B },
        { PlayerKey.Run, Button.A },
        { PlayerKey.Jump, Button.X },
        { PlayerKey.BreakWand, Button.Y }
    };

    static readonly Dictionary<PlayerKey, Axis> VirtualKeyMapping = new Dictionary<PlayerKey, Axis>() {
        { PlayerKey.UseLeft, Axis.LeftTrigger },
        { PlayerKey.UseRight, Axis.RightTrigger },
    };

    // Private members.

    JoystickInputWrapper m_Wrapper;
    Dictionary<PlayerKey, KeyState> m_VirtualKeyStates;

    // API.

    public bool GetKeyDown( PlayerKey key )
    {
        if( VirtualKeyMapping.ContainsKey( key ) )
        {
            return m_VirtualKeyStates[ key ] == KeyState.Down;
        }
        return m_Wrapper.GetButtonDown( (int) KeyMapping[key] );
    }

    public bool GetKey( PlayerKey key )
    {
        if( VirtualKeyMapping.ContainsKey( key ) )
        {
            var state = m_VirtualKeyStates[ key ];
            return state == KeyState.Held || state == KeyState.Down;
        }
        return m_Wrapper.GetButton( (int) KeyMapping[key] );
    }

    public bool GetKeyUp( PlayerKey key )
    {
        if( VirtualKeyMapping.ContainsKey( key ) )
        {
            return m_VirtualKeyStates[ key ] == KeyState.Up;
        }
        return m_Wrapper.GetButtonUp( (int) KeyMapping[key] );
    }

    public Vector2 Movement()
    {
        return new Vector2(
            m_Wrapper.GetAxis( (int) Axis.LeftX ),
            -m_Wrapper.GetAxis( (int) Axis.LeftY )
        );
    }

    public Vector2 Aim()
    {
        return new Vector2(
            m_Wrapper.GetAxis( (int) Axis.RightX ),
            -m_Wrapper.GetAxis( (int) Axis.RightY )
        );
    }

    // Unity events.

    void Awake()
    {
        // Disable until we hear back from config.
        this.enabled = false;

        Component<Player>().Config.Watch( HandleConfigure );
    }

    void OnDestroy()
    {
        Component<Player>().Config.Unwatch( HandleConfigure );
    }

    void HandleConfigure( PlayerConfig config )
    {
        int joystickNumber = -1;
        switch( config.ControlType )
        {
            case ControlType.Joystick1:
                joystickNumber = 1;
                break;
            case ControlType.Joystick2:
                joystickNumber = 2;
                break;
            case ControlType.Joystick3:
                joystickNumber = 3;
                break;
            case ControlType.Joystick4:
                joystickNumber = 4;
                break;
        }
        if( joystickNumber == -1 )
        {
            this.enabled = false;
        }
        else
        {
            m_Wrapper = new JoystickInputWrapper( joystickNumber );
            m_VirtualKeyStates = VirtualKeyMapping.Keys.ToDictionary(
                k => k,
                k => KeyState.None
            );
            this.enabled = true;
        }
    }

    public void Update()
    {
        foreach( var mapping in VirtualKeyMapping )
        {
            var currentState = m_VirtualKeyStates[ mapping.Key ];
            var isDown = m_Wrapper.GetAxis( (int) mapping.Value ) > 0f;

            KeyState newState;
            if( isDown )
            {
                newState = currentState == KeyState.Down
                    ? KeyState.Held
                    : KeyState.Down;
            }
            else
            {
                newState = currentState == KeyState.Held || currentState == KeyState.Down
                    ? KeyState.Up
                    : KeyState.None;
            }
            m_VirtualKeyStates[ mapping.Key ] = newState;
        }
    }
}