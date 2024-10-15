using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_Move : MonoBehaviour
{
    public float speed = 6f;
    public float jumpPower = 5f;
    public float gravity = 10f;
    public float coyoteTime = 0.2f; // Tiempo de coyote jump

    public bool isIn2D = true;  // Controla si estamos en 2D o 2.5D

    private float jumpBufferTime = 0.2f; // Tiempo de buffer para el salto
    private float jumpBufferCounter;
    private float coyoteTimer;
    private Vector3 moveDirection = Vector3.zero;
    private bool canJump = false;
    private CharacterController characterController;
    private Vector3 resetVector = new Vector3(1, 1, 0);

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
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

        float movementDirectionY = moveDirection.y;

        // Movimiento en X y Z (solo en 2.5D), sin Y que se maneja en salto
        Vector3 move = (right * curSpeedX) + (forward * curSpeedZ);

        // Normalizamos el vector de movimiento para evitar que se mueva más rápido en diagonal
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // Multiplicamos por la velocidad
        moveDirection = move * speed;
        // Reaplicar el movimiento en Y para caída o salto
        moveDirection.y = movementDirectionY;
        ;
        if(isIn2D)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Aplicar movimiento en el CharacterController
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleJumping()
    {
        // Comprobar si el personaje está tocando el suelo
        if (characterController.isGrounded)
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
        if(jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime; // Decrementar el contador de salto buffer
            if (canJump)
            {
                moveDirection.y = jumpPower;
                canJump = false;
            }
        }
        if (!characterController.isGrounded)
        {
            // Aplica gravedad mientras no está en el suelo
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    // Cambiar entre perspectiva 2D y 2.5D
    public void SwitchPerspective()
    {
        isIn2D = !isIn2D;
    }
    // Detectar colisiones y ajustar posición si colisiona en la parte superior de un objeto
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Comprobar si la colisión fue en la parte superior del objeto
        if (hit.normal.y < -0.5f) // Si la normal de contacto indica que fue desde arriba
        {
            // Ajustar la posición del jugador en la dirección Y
            transform.position = new Vector3(transform.position.x, hit.point.y + characterController.height / 2, transform.position.z);
            canJump = true; // Permitir el salto nuevamente
        }
    }
}
