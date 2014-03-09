public enum ControlType
{
    None,
    Keyboard,
    Joystick1,
    Joystick2,
    Joystick3,
    Joystick4
}

[System.Serializable]
public class PlayerConfig
{
    public int PlayerNumber;
    public ControlType ControlType;
}
