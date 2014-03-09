using UnityEngine;

public class PlayerStartFactory : MonoBehaviour
{
    public PlayerStart PlayerStartPrefab;

    public Transform Build( int playerNumber, Vector3 position )
    {
        var start = Instantiate( PlayerStartPrefab, position, Quaternion.identity ) as PlayerStart;
        start.Config.PlayerNumber = playerNumber;
        start.Config.ControlType = ControlType.Joystick1;
        return start.transform;
    }
}