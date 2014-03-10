using UnityEngine;
using System.Collections;
using UnityObjectRetrieval;

public class PlayerStart : MonoBehaviour
{
    public Player PlayerPrefab;
    public PlayerConfig Config;

    GameManager m_GameManager;

    void Awake()
    {
        m_GameManager = Scene.Object<GameManager>();
    }

    void OnEnable()
    {
        m_GameManager.EnterStateEventMap.AddHandler( GameState.Intro, HandleStartIntro );
    }

    void OnDisable()
    {
        m_GameManager.EnterStateEventMap.RemoveHandler( GameState.Intro, HandleStartIntro );
    }

    void HandleStartIntro()
    {
        Spawn();
    }

    void Spawn()
    {
        var player = Instantiate(
            PlayerPrefab,
            transform.position + Vector3.up * 0.1f,
            Quaternion.identity ) as Player;

        player.Configure( Config );
    }
}