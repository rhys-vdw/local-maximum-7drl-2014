using UnityEngine;
using UnityObjectRetrieval;

public class PlayerInput : ExtendedMonoBehaviour
{
    IInputMapping m_InputMapping;

    public void Awake()
    {
        var player = Component<Player>();
        player.ConfigureEvent += HandleConfigure;
    }

    void HandleConfigure( PlayerConfig config )
    {
        switch( config.ControlType )
        {
            case ControlType.Joystick1:
            case ControlType.Joystick2:
            case ControlType.Joystick3:
            case ControlType.Joystick4:
                m_InputMapping = Component<JoystickInputMapping>();
                break;
            default:
                throw new System.InvalidOperationException( string.Format(
                    "Unrecognized control type: {0}", config.ControlType
                ) );
        }
    }
}