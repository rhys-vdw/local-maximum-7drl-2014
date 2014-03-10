using UnityEngine;
using System.Linq;
using UnityObjectRetrieval;

public class SpriteSheet : ScriptableObject
{
    [System.Serializable]
    public class SpriteDefinition
    {
        public string Name;
        public int X;
        public int Y;
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

    public void AutoApplyRandom( Component component )
    {
        var renderer = component.SelfDescendants().Component<Renderer>();
        var filter = component.SelfDescendants().Component<MeshFilter>();
        Apply( Sprites.RandomElement(), renderer, filter.mesh );
    }

    public void ApplyRandom( Renderer renderer, Mesh mesh )
    {
        Apply( Sprites.RandomElement(), renderer, mesh );
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
        float sheetWidth = 1f / Width;
        float sheetHeight = 1f / Height;

        renderer.sharedMaterial = Material;

        var uvs = mesh.uv;
        for( int i = 0; i < mesh.vertexCount; i++ )
        {
            uvs[i] = new Vector2(
                (sprite.X + uvs[i].x) * sheetWidth,
                (sprite.Y + uvs[i].y) * sheetHeight
            );
        }
        mesh.uv = uvs;
    }
}