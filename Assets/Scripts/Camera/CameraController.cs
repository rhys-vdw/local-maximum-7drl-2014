using UnityEngine;
using UnityObjectRetrieval;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class CameraController : ExtendedMonoBehaviour
{
    public float StartingDepth = 2f;
    public float Height = 10f;
    public float Angle = 45f;

    float m_Speed = 1f;
    Camera m_Camera;
    Transform m_Transform;

    void Awake()
    {
        m_Camera = Component<Camera>();
        m_Transform = Component<Transform>();
        Scene.Object<MapBuilder>().BuildCompleteEvent += HandleBuildComplete;
    }

    void HandleBuildComplete( MapBuilder map )
    {
        m_Camera.orthographic = true;
        m_Camera.orthographicSize =
            (map.Width * m_Camera.pixelHeight) /
            (2 * Screen.width);

        m_Transform.position = map.transform.position + new Vector3(
            (map.Width - 1) / 2,
            Height,
            StartingDepth
        );
        m_Transform.rotation = Quaternion.Euler(
            Angle, 0f, 0f
        );
    }

    void FixedUpdate()
    {
        m_Transform.position += Vector3.forward * m_Speed * Time.fixedDeltaTime;
    }
}
