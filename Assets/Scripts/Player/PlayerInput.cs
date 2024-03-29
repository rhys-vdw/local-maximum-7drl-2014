using UnityEngine;
using UnityObjectRetrieval;

public class PlayerInput : ExtendedMonoBehaviour
{
    IInputMapping m_InputMapping;

    // API.

    public bool GetKeyDown( PlayerKey key )
    {
        return m_InputMapping == null ? false : m_InputMapping.GetKeyDown( key );
    }

    public bool GetKey( PlayerKey key )
    {
        return m_InputMapping == null ? false : m_InputMapping.GetKey( key );
    }

    public bool GetKeyUp( PlayerKey key )
    {
        return m_InputMapping == null ? false : m_InputMapping.GetKeyUp( key );
    }

    public Vector2 Movement()
    {
        return m_InputMapping == null ? Vector2.zero : m_InputMapping.Movement();
    }

    public Vector2 Aim()
    {
        return m_InputMapping == null ? Vector2.zero : m_InputMapping.Aim();
    }

    // Unity events.

    void OnEnable()
    {
        var player = Component<Player>();
        player.Config.Watch( HandleConfigure );
    }

    void HandleConfigure( PlayerConfig config )
    {
        switch( config.ControlType )
        {
            case ControlType.Joystick1:
            case ControlType.Joystick2:
            case ControlType.Joystick3:
            case ControlType.Joystick4:
                m_InputMapping = gameObject.AddComponent<JoystickInputMapping>();
                break;
            default:
                throw new System.InvalidOperationException( string.Format(
                    "Unrecognized control type: {0}", config.ControlType
                ) );
        }
    }
}