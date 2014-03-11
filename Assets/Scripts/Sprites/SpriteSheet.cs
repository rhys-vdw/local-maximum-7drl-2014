using UnityEngine;
using System.Linq;
using UnityObjectRetrieval;
using System.Collections.Generic;
using System;

// using RotationMap = Dictionary<SpriteRotation, Func<Vector2, Vector2>>;
using RotationMap = System.Collections.Generic.Dictionary<SpriteRotation, System.Func<UnityEngine.Vector2, UnityEngine.Vector2>>;

public enum SpriteRotation
{
    None,
    FlipHorizontal,
    FlipVertical,
    RotateCW,
    RotateCCW,
    Rotate180,
    Random
}

public class SpriteSheet : ScriptableObject
{
    readonly static RotationMap RotationFunctions = new RotationMap
    {
        { SpriteRotation.None,           uv => uv                                },
        { SpriteRotation.FlipHorizontal, uv => new Vector2( 1 - uv.x,     uv.y ) },
        { SpriteRotation.FlipVertical,   uv => new Vector2(     uv.x, 1 - uv.y ) },
        { SpriteRotation.RotateCW,       uv => new Vector2( 1 - uv.y,     uv.x ) },
        { SpriteRotation.RotateCCW,      uv => new Vector2(     uv.y, 1 - uv.x ) },
        { SpriteRotation.Rotate180,      uv => new Vector2( 1 - uv.x, 1 - uv.y ) },
    };

    [System.Serializable]
    public class SpritePosition
    {
        public int X;
        public int Y;

        public Vector2 ToVector2()
        {
            return new Vector2( X, Y );
        }
    }

    [System.Serializable]
    public class SpriteDefinition
    {
        public string Name;
        public SpritePosition[] Positions;
        public SpriteRotation Rotation;
    }

    public Material Material;

    public SpriteDefinition[] Sprites;
    public int Width = 1;
    public int Height = 1;

    public void AutoApply( string name, Component component )
    {
        var renderer = component.SelfDescendants().Component<Renderer>();
        var filter = component.SelfDescendants().Component<MeshFilter>();
        Apply( name, renderer, filter.mesh );
    }

    public void Apply( string name, MeshRenderer renderer )
    {
        Apply( name, renderer, renderer.Component<MeshFilter>().mesh );
    }

    public void Apply( string name, Renderer renderer, Mesh mesh )
    {
        var sprite = Sprites.FirstOrDefault( s => s.Name == name );

        if( sprite == null ) throw new System.InvalidOperationException(
            string.Format( "Could not find sprite named: '{0}'", name )
        );

        Apply( sprite, renderer, mesh );
    }

    void Apply( SpriteDefinition sprite, Renderer renderer, Mesh mesh )
    {
        renderer.sharedMaterial = Material;

        var position = sprite.Positions.RandomElement();
        var gridSize = new Vector2( 1f / Width, 1f / Height );

        var rotateFunc = sprite.Rotation == SpriteRotation.Random
            ? RotationFunctions.Values.ToList().RandomElement()
            : RotationFunctions[ sprite.Rotation ];

        var uvs = mesh.uv;
        for( int i = 0; i < mesh.vertexCount; i++ )
        {
            uvs[i] = Vector2.Scale(
                position.ToVector2() + rotateFunc(uvs[i]),
                gridSize
            );
        }
        mesh.uv = uvs;
    }
}