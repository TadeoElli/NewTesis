using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_Move : MonoBehaviour
{
    [SerializeField]private float speed = 6f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float coyoteTime = 0.2f; // Tiempo de coyote jump

    private bool isIn2D = false;  // Controla si estamos en 2D o 2.5D

    private float jumpBufferTime = 0.2f; // Tiempo de buffer para el salto
    private float jumpBufferCounter;
    private float coyoteTimer;
    private Vector3 moveDirection = Vector3.zero;
    private bool canJump = false;
    private Rigidbody rb;

    private float originalZPosition; // Para guardar la posición Z original

    [SerializeField] private float stoppingDrag = 20f; // Drag para frenar rápidamente

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Evitar que el Rigidbody rote
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
        HandleJumping();
    }

    void HandleMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Velocidad de movimiento en X (Horizontal) y Z (Profundidad) si está en 2.5D
        float curSpeedX = speed * Input.GetAxis("Horizontal");
        float curSpeedZ = isIn2D ? 0 : speed * Input.GetAxis("Vertical");

        // Movimiento en X y Z (solo en 2.5D)
        Vector3 move = (right * curSpeedX) + (forward * curSpeedZ);

        // Normalizamos el vector de movimiento para evitar que se mueva más rápido en diagonal
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // Si no hay input, aplicar desaceleración rápida
        if (Mathf.Abs(curSpeedX) < 0.01f && Mathf.Abs(curSpeedZ) < 0.01f)
        {
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, stoppingDrag * Time.deltaTime), rb.velocity.y, Mathf.Lerp(rb.velocity.z, 0, stoppingDrag * Time.deltaTime));
        }
        else
        {
            // Aplicar movimiento
            rb.velocity = new Vector3(move.x * speed, rb.velocity.y, move.z * speed);
        }

        // Si estamos en modo 2D, fijamos la posición Z en 0
        if (isIn2D)
        {
            rb.position = new Vector3(rb.position.x, rb.position.y, 0f);
        }
    }

    void HandleJumping()
    {
        // Comprobar si el personaje está tocando el suelo
        if (IsGrounded())
        {
            // Reinicia el temporizador de coyote jump
            canJump = true;
            coyoteTimer = coyoteTime;
        }
        else
        {
            // Reduce el temporizador de coyote jump
            coyoteTimer -= Time.deltaTime;
            if (coyoteTimer <= 0)
            {
                canJump = false;
            }
        }

        // Realiza el salto si presiona el botón y aún puede saltar (coyote jump o suelo)
        if (Input.GetButtonDown("Jump") && canJump)
        {
            jumpBufferCounter = jumpBufferTime; // Activar el contador de buffer al presionar el salto
        }

        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime; // Decrementar el contador de salto buffer
            if (canJump)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
                canJump = false;
            }
        }

        // Si no está en el suelo, aplicar gravedad extra manualmente
        if (!IsGrounded())
        {
            rb.velocity += Vector3.down * gravity * Time.deltaTime;
        }
    }

    // Método para comprobar si el personaje está en el suelo
    bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, 1.1f); // Verifica una pequeña distancia para detectar si está en el suelo
    }

    // Cambiar entre perspectiva 2D y 2.5D
    public void SwitchPerspective()
    {
        isIn2D = !isIn2D;

        if (isIn2D)
        {
            originalZPosition = rb.position.z;
            rb.position = new Vector3(rb.position.x, rb.position.y, 0f);
        }
        else
        {
            rb.position = new Vector3(rb.position.x, rb.position.y, originalZPosition);
        }
    }
}

