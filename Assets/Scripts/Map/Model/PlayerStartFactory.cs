using UnityEngine;
using UnityObjectRetrieval;

public class PlayerStartFactory : MonoBehaviour
{
    public PlayerStart PlayerStartPrefab;

    public Transform Build( int playerNumber, Vector3 position )
    {
        var start = Instantiate( PlayerStartPrefab, position, Quaternion.identity ) as PlayerStart;

        var config = Scene.Object<ConfigManager>();

        start.Config.PlayerNumber = playerNumber;
        start.Config.ControlType = config.ControlTypes[playerNumber];

        return start.transform;
    }
}