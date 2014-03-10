using UnityEngine;
using UnityObjectRetrieval;

public class PlayerMovement : ExtendedMonoBehaviour
{
    public float m_WalkSpeed = 2f;
    public float m_RunSpeed = 4f;

    PlayerInput m_Input;
    CharacterController m_Controller;
    Transform m_Transform;
    float m_Height;

    public bool IsRunning { get; private set; }

    void Awake()
    {
        m_Input = Component<PlayerInput>();
        m_Controller = Component<CharacterController>();
        m_Transform = transform;
        m_Height = m_Transform.position.y;

        IsRunning = false;
    }

    void FixedUpdate()
    {
        IsRunning = m_Input.GetKey( PlayerKey.Run );

        var speed = IsRunning ? m_RunSpeed : m_WalkSpeed;
        var movement = m_Input.Movement();
        var step = new Vector3(
            movement.x * speed * Time.fixedDeltaTime,
            0f,
            movement.y * speed * Time.fixedDeltaTime
        );

        m_Controller.Move( step );
        var pos = m_Transform.position;
        pos.y = m_Height;
        m_Transform.position = pos;
    }
}