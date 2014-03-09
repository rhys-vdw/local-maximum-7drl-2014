using UnityEngine;
using System.Collections;
using UnityObjectRetrieval;

public class PlayerStart : MonoBehaviour
{
    public Player PlayerPrefab;
    public PlayerConfig Config;

    GameManager m_GameManager;

    void OnEnable()
    {
        m_GameManager = Scene.Object<GameManager>();
        m_GameManager.StartIntroEvent += HandleStartIntro;
    }

    void OnDisable()
    {
        m_GameManager.StartIntroEvent -= HandleStartIntro;
    }

    void HandleStartIntro()
    {
        Spawn();
    }

    void Spawn()
    {
        var player = Instantiate(
            PlayerPrefab,
            transform.position,
            Quaternion.identity ) as Player;

        player.Configure( Config );
    }
}