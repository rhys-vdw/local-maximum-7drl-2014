using UnityEngine;
using System.Linq;
using UnityObjectRetrieval;

public class SpriteSheet : ScriptableObject
{
    [System.Serializable]
    public class SpritePosition
    {
        public int X;
        public int Y;
    }

    [System.Serializable]
    public class SpriteDefinition
    {
        public string Name;
        public SpritePosition[] Positions;
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
        var position = sprite.Positions.RandomElement();

        float sheetWidth = 1f / Width;
        float sheetHeight = 1f / Height;

        renderer.sharedMaterial = Material;

        var uvs = mesh.uv;
        for( int i = 0; i < mesh.vertexCount; i++ )
        {
            uvs[i] = new Vector2(
                (position.X + uvs[i].x) * sheetWidth,
                (position.Y + uvs[i].y) * sheetHeight
            );
        }
        mesh.uv = uvs;
    }
}