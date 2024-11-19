using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public Rigidbody player;
    public Animator animator;

    public float velocidad = 5f;           
    public float velRotacion = 200f;       
    public float m_interpolation = 10f;    
    private bool adelante;
    private bool atras;
    private bool derecha;
    private bool izquierda;
    private bool rotDerecha;
    private bool rotIzquierda;
    private float m_currentV = 0f;         
    private float m_currentH = 0f;         
    private float moveSpeed;
    private bool m_isGrounded;             
    private List<Collider> m_collisions = new List<Collider>();

    public enum ControlMode
    {
        Tank,
        Direct
    }

    public ControlMode controlMode = ControlMode.Direct;

    //Calcula el movimiento según las teclas presionadas
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

    // Indica hacia dónde se debe mover el jugador
    public void HaciaAdelante() { adelante = true; }
    public void HaciaAtras() { atras = true; }
    public void HaciaIzquierda() { izquierda = true; }
    public void HaciaDerecha() { derecha = true; }
    public void RotacionDerecha() { rotDerecha = true; }
    public void RotacionIzquierda() { rotIzquierda = true; }
    public void sinFuncion()
    {
        adelante = false;
        atras = false;
        derecha = false;
        izquierda = false;
        rotDerecha = false;
        rotIzquierda = false;
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

        // detecta si el player esta tocando el suelo
        animator.SetBool("Grounded", m_isGrounded);

        switch (controlMode)
        {
            case ControlMode.Tank:
                TankControlUpdate();
                break;

            case ControlMode.Direct:
                DirectControlUpdate();
                break;
        }

        moveSpeed = Mathf.Abs(m_currentV) + Mathf.Abs(m_currentH);
        animator.SetFloat("MoveSpeed", moveSpeed);
    }

    // Control en modo Tank (movimiento independiente de rotación)
    //void TankControlUpdate()
    //{
    //    CalculateMovement();

    //    if (m_currentV != 0)
    //    {
    //        Vector3 moveDirection = transform.forward * m_currentV;
    //        player.MovePosition(player.position + moveDirection * velocidad * Time.deltaTime);
    //    }

    //    if (m_currentH != 0)
    //    {
    //        Vector3 moveDirection = transform.right * m_currentH;
    //        player.MovePosition(player.position + moveDirection * velocidad * Time.deltaTime);
    //    }

    //    if (rotDerecha)
    //    {
    //        transform.Rotate(Vector3.up * velRotacion * Time.deltaTime);
    //    }
    //    if (rotIzquierda)
    //    {
    //        transform.Rotate(-Vector3.up * velRotacion * Time.deltaTime);
    //    }
    //}

    //void DirectControlUpdate()
    //{
    //    CalculateMovement();

    //    if (m_currentV != 0)
    //    {
    //        Vector3 moveDirection = Camera.main.transform.forward * m_currentV;
    //        moveDirection.y = 0;
    //        player.MovePosition(player.position + moveDirection * velocidad * Time.deltaTime);
    //    }

    //    if (m_currentH != 0)
    //    {
    //        Vector3 moveDirection = Camera.main.transform.right * m_currentH;
    //        moveDirection.y = 0;
    //        player.MovePosition(player.position + moveDirection * velocidad * Time.deltaTime);
    //    }

    //    Vector3 targetDirection = Camera.main.transform.forward * m_currentV + Camera.main.transform.right * m_currentH;
    //    if (targetDirection != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_interpolation);
    //    }
    //}
    void TankControlUpdate()
    {
        CalculateMovement();

        if (m_currentV != 0)
        {
            Vector3 moveDirection = transform.forward * m_currentV;
            player.MovePosition(player.position + moveDirection * velocidad * Time.deltaTime);
        }

        if (m_currentH != 0)
        {
            Vector3 moveDirection = transform.right * m_currentH;
            player.MovePosition(player.position + moveDirection * velocidad * Time.deltaTime);
        }

        if (rotDerecha)
        {
            transform.Rotate(Vector3.up * velRotacion * Time.deltaTime);  // Rotación derecha
        }
        if (rotIzquierda)
        {
            transform.Rotate(-Vector3.up * velRotacion * Time.deltaTime); // Rotación izquierda
        }
    }

    void DirectControlUpdate()
    {
        CalculateMovement();

        if (m_currentV != 0)
        {
            Vector3 moveDirection = Camera.main.transform.forward * m_currentV;
            moveDirection.y = 0; 
            player.MovePosition(player.position + moveDirection * velocidad * Time.deltaTime);
        }

        if (m_currentH != 0)
        {
            Vector3 moveDirection = Camera.main.transform.right * m_currentH;
            moveDirection.y = 0;
            player.MovePosition(player.position + moveDirection * velocidad * Time.deltaTime);
        }

        Vector3 targetDirection = Camera.main.transform.forward * m_currentV + Camera.main.transform.right * m_currentH;

        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_interpolation);
        }
    }

}

