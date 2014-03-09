using UnityEngine;
using UnityObjectRetrieval;
using IEnumerator = System.Collections.IEnumerator;
using System;

/*
public enum State
{
    None,
    Generating,
    Intro,
    Playing,
}
*/

public class GameManager : MonoBehaviour
{
    public MapGenerationOptions MapGenerationOptions = new MapGenerationOptions {
        Width = 24,
        Height = 100
    };

    public event Action StartIntroEvent;

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        StartCoroutine( StartGameCoroutine() );
    }

    IEnumerator StartGameCoroutine()
    {
        // Send event here.
        yield return null;

        var generator = Scene.Object<MapGenerator>();
        var builder = Scene.Object<MapBuilder>();

        var map = generator.GenerateMap( MapGenerationOptions );
        builder.Build( map );

        StartIntro();
    }

    void StartIntro()
    {
        StartCoroutine( StartIntroCoroutine() );
    }

    IEnumerator StartIntroCoroutine()
    {
        if( StartIntroEvent != null ) StartIntroEvent();
        yield return new WaitForSeconds( 0.2f );
    }
}