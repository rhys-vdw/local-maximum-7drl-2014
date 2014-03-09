using UnityEngine;

public class LogJoystickInputConfig : MonoBehaviour
{
    public int ControllerCount = 4;
    public int AxisCount = 21;
    public float Dead = 0.00100000005f;

    void Awake()
    {
        string x = "";
        for( int j = 1; j <= ControllerCount; j++ )
        {
            for( int i = 0; i < 21; i++ )
            {
                x += string.Format(
@"  - serializedVersion: 3
    m_Name: Joystick{0}Axis{1}
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: {2}
    sensitivity: 1
    snap: 0
    invert: 0
    type: 2
    axis: {1}
    joyNum: {0}
", j, i, Dead );
            }
        }
        Debug.Log( x );
    }
}