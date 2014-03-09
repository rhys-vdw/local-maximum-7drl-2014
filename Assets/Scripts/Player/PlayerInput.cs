using UnityEngine;
using UnityObjectRetrieval;

public class PlayerInput : ExtendedMonoBehaviour
{
    IInputMapping m_InputMapping;

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