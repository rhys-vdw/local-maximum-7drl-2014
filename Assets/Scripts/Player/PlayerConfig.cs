using UnityEngine;

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
public class StartingItems
{
    public string LeftHand;
    public string RightHand;
    public string Wearable;
    public string Backpack;
}

[System.Serializable]
public class PlayerConfig
{

    [HideInInspector] public int PlayerNumber;
    public ControlType ControlType;
    public StartingItems StartingItems;
}
