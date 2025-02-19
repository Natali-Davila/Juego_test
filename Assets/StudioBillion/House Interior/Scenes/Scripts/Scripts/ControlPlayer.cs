using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public Rigidbody player;
    public Animator animator;

    [SerializeField] private float m_moveSpeed = 5;
    public float m_interpolation = 10f;    
    private bool adelante;
    private bool atras;
    private bool derecha;
    private bool izquierda;
    private float m_currentV = 0f;         
    private float m_currentH = 0f;         
    private float moveSpeed;
    private bool m_isGrounded;
    private readonly float m_walkScale = 0.33f;
    public ControlMode controlMode = ControlMode.Direct;

    private List<Collider> m_collisions = new List<Collider>();
    private Vector3 m_currentDirection = Vector3.zero;

    public enum ControlMode
    {
        Tank,
        Direct
    }

    private void CalculateMovement()
    {
        if (adelante)
        {
            m_currentV = 1f;
        }
        else if (atras)
        {
            m_currentV = -1f;
        }
        else
        {
            m_currentV = 0f;
        }

        if (derecha)
        {
            m_currentH = 1f;
        }
        else if (izquierda)
        {
            m_currentH = -1f;
        }
        else
        {
            m_currentH = 0f;
        }

        m_currentV = Mathf.Lerp(m_currentV, m_currentV, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, m_currentH, Time.deltaTime * m_interpolation);
    }

    public void HaciaAdelante() { adelante = true; }
    public void HaciaAtras() { atras = true; }
    public void HaciaIzquierda() { izquierda = true; }
    public void HaciaDerecha() { derecha = true; }
    public void sinFuncion()
    {
        adelante = false;
        atras = false;
        derecha = false;
        izquierda = false;
    }

    // Detección de colisiones
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    void Update()
    {
        player.interpolation = RigidbodyInterpolation.Interpolate;
        player.collisionDetectionMode = CollisionDetectionMode.Continuous;
        player.useGravity = true;

        animator.SetBool("Grounded", m_isGrounded);

        switch (controlMode)
        {
            case ControlMode.Direct:
                DirectControlUpdate();
                break;
        }

        moveSpeed = Mathf.Abs(m_currentV) + Mathf.Abs(m_currentH);
        animator.SetFloat("MoveSpeed", moveSpeed);
    }
    void DirectControlUpdate()
    {
        CalculateMovement();

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= m_walkScale;
            h *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            animator.SetFloat("MoveSpeed", direction.magnitude);
        }
    }
}