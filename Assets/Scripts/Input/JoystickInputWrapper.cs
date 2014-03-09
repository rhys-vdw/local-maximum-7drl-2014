using UnityEngine;

public class JoystickInputWrapper
{
    KeyCode[] m_Buttons;
    string[] m_Axes;

    const int ButtonCount = 10;
    const int AxisCount = 11;

    public JoystickInputWrapper( int joystickNumber )
    {
        if( joystickNumber < 1 || joystickNumber > 4 )
        {
            throw new System.ArgumentException( "joystickNumber" );
        }

        m_Buttons = new KeyCode[ ButtonCount ];
        for( int i = 0; i < ButtonCount; i++ )
        {
            m_Buttons[i] = EnumUtil.Parse<KeyCode>( string.Format(
                "Joystick{0}Button{1}", joystickNumber, i
            ) );
        }

        m_Axes = new string[ AxisCount ];
        for( int i = 0; i < AxisCount; i++ )
        {
            m_Axes[i] = string.Format(
                "Joystick{0}Axis{1}", joystickNumber, i
            );
        }
    }

    public float GetAxis( int number )
    {
        return Input.GetAxis( m_Axes[ number ] );
    }

    public float GetAxisRaw( int number )
    {
        return Input.GetAxisRaw( m_Axes[ number ] );
    }

    public bool GetButton( int number )
    {
        return Input.GetKey( m_Buttons[ number ] );
    }

    public bool GetButtonDown( int number )
    {
        return Input.GetKeyDown( m_Buttons[ number ] );
    }

    public bool GetButtonUp( int number )
    {
        return Input.GetKeyUp( m_Buttons[ number ] );
    }
}
