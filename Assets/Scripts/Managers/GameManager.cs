using UnityEngine;
using UnityObjectRetrieval;
using IEnumerator = System.Collections.IEnumerator;
using System;
using EventTools;

public enum GameState
{
    None,
    Generating,
    Intro,
    Playing
}

public class GameManager : MonoBehaviour
{
    public EventMap<GameState> EnterStateEventMap = new EventMap<GameState>();
    public EventMap<GameState> ExitStateEventMap = new EventMap<GameState>();

    GameState m_State;

    void SetState( GameState state )
    {
        if( state != m_State )
        {
            ExitStateEventMap.FireEvent( m_State );
            m_State = state;
            EnterStateEventMap.FireEvent( m_State );
        }
    }

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        SetState( GameState.Generating );
        StartCoroutine( StartGameCoroutine() );
    }

    IEnumerator StartGameCoroutine()
    {
        yield return null;

        var generator = Scene.Object<MapGenerator>();
        var builder = Scene.Object<MapBuilder>();

        var genOptions = Scene.Object<ConfigManager>().MapGenerationOptions;
        var map = generator.GenerateMap( genOptions );
        Debug.Log( map.ToVisualString() );
        builder.Build( map );

        StartIntro();
    }

    void StartIntro()
    {
        SetState( GameState.Intro );
        StartCoroutine( StartIntroCoroutine() );
    }

    IEnumerator StartIntroCoroutine()
    {
        yield return new WaitForSeconds( 0.2f );
        StartPlaying();
    }

    void StartPlaying()
    {
        SetState( GameState.Playing );
    }
}