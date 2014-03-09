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

    public Texture Texture;

    public SpriteDefinition[] Sprites;
    public int Width = 1;
    public int Height = 1;

    public void  AutoApply( string name, Component component )
    {
        var renderer = component.SelfDescendants().Component<MeshRenderer>();
        var filter = component.SelfDescendants().Component<MeshFilter>();
        Apply( name, renderer.material, filter.mesh );
    }

    public void  Apply( string name, Material material, Mesh mesh, string textureName = "_MainTex" )
    {
        float sheetWidth = 1f / Width;
        float sheetHeight = 1f / Height;

        var sprite = Sprites.FirstOrDefault( s => s.Name == name );

        if( sprite == null ) throw new System.InvalidOperationException(
            string.Format( "Could not find sprite named: '{0}'", name )
        );

        material.SetTexture( textureName, Texture );

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