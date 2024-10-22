using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;

    Vector3 moveDirection;
    [SerializeField] Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("MovementSpeed Speed")]
    public float movementSpeed = 7;
    public float rotationSpeed = 15;
    private float originalZPosition; // Para almacenar la posición Z original del jugador en 3D
    [Header("Movement Flags")]
    public bool isGrounded = false;
    public bool isJumping = false;
    public bool isNearEdge = false;  // Flag para detectar si el jugador está cerca de un borde

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public float maxDistance = 1f;
    public LayerMask groundLayer;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    // Coyote Jump
    [Header("Coyote Jump Settings")]
    public float coyoteTime = 0.2f;  // Time window after leaving the ground to still jump
    private float coyoteTimer;        // Timer to track coyote jump

    // Jump Buffering
    [Header("Jump Buffer Settings")]
    public float jumpBufferTime = 0.2f; // Time to buffer a jump input
    private float jumpBufferTimer;      // Timer to track buffered jumps

    /*[Header("Edge Detection Settings")]
    public float edgeRayLength = 1f;  // Longitud del raycast para detectar bordes
    public Transform[] edgeRaycastOrigins;  // Puntos de origen de los raycasts laterales
    public float edgeCorrectionSpeed = 5f; // Velocidad para ajustar la posición del jugador
    */
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    #region Movement
    private void HandleMovement()
    {
        // Si está en modo 2D, solo permitir movimiento en el eje X
        if (inputManager.isOn2D)
        {
            Vector3 currentVelocity = playerRigidbody.velocity;

            // Solo permitir movimiento en el eje X
            currentVelocity.x = inputManager.horizontalInput * movementSpeed;
            currentVelocity.z = 0; // Mantener siempre el Z en 0 en modo 2D

            // Fijar la posición del jugador en el eje Z a 0
            playerRigidbody.position = new Vector3(playerRigidbody.position.x, playerRigidbody.position.y, 0);

            playerRigidbody.velocity = currentVelocity;
        }
        else
        {
            // Movimiento normal en 3D
            moveDirection = cameraObject.forward * inputManager.verticalInput;
            moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
            moveDirection.Normalize();
            moveDirection.y = 0;
            moveDirection = moveDirection * movementSpeed;

            Vector3 currentVelocity = playerRigidbody.velocity;
            currentVelocity.x = moveDirection.x;
            currentVelocity.z = moveDirection.z;

            playerRigidbody.velocity = currentVelocity; // Mantén la velocidad vertical (Y) intacta
        }
    }

    public void SwitchTo2D()
    {
        // Guardar la posición Z original antes de cambiar a 2D
        originalZPosition = playerRigidbody.position.z;

    }

    public void SwitchTo3D()
    {
        // Restaurar la posición Z original cuando volvemos a 3D
        Vector3 currentPosition = playerRigidbody.position;
        playerRigidbody.position = new Vector3(currentPosition.x, currentPosition.y, originalZPosition);

    }
    #endregion

    #region Rotation
    private void HandleRotation()
    {
        if(isJumping)
            return;
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput ;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
    #endregion
    #region Falling and Jumping
    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;
        bool isGroundDetected = Physics.SphereCast(rayCastOrigin, 0.1f, -Vector3.up, out hit, maxDistance, groundLayer);

        // Si no hay suelo debajo, verificar bordes
        if (isGroundDetected && !hit.collider.isTrigger)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            isGrounded = true;
            isNearEdge = false;  // Si está en el suelo, no está en un borde
            coyoteTimer = coyoteTime;  // Resetear el temporizador de coyote jump
            inAirTimer = 0;
            isJumping = false;
        }
        else
        {
            //isNearEdge = DetectEdge();  // Verificar si está cerca de un borde

            //if (!isNearEdge)
            //{
                // Si no hay suelo ni bordes, el jugador está cayendo
                inAirTimer = inAirTimer + Time.deltaTime;
                playerRigidbody.AddForce(transform.forward * leapingVelocity);
                playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
            //}
        }

        if (!isGroundDetected && !isNearEdge)
        {
            isGrounded = false;
        }
    }
    public void HandleJumping()
    {
        // Verificar Coyote Jump
        if (coyoteTimer > 0 && !isJumping)
        {
            isJumping = true;
            float jumpingVelocity = Mathf.Sqrt(-2f * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = playerRigidbody.velocity;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;

            // Reiniciar coyote jump solo después de saltar
            coyoteTimer = 0;
        }
    }
    #endregion
    // Actualiza el temporizador de coyote jump y jump buffering
    private void Update()
    {
        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime; // Cuenta regresiva de coyote jump
        }
        jumpBufferTimer -= Time.deltaTime; // Cuenta regresiva de jump buffering
    }

    #region JumpBuffering

    private void HandleJumpBuffering()
    {
        // Jump buffering: si se presiona el salto y el temporizador de buffer no ha expirado
        if (jumpBufferTimer > 0)
        {
            HandleJumping();
            jumpBufferTimer = 0;
        }
    }
    // Llamado desde InputManager cuando se presiona el botón de salto
    public void BufferJump()
    {
        jumpBufferTimer = jumpBufferTime;
    }

    #endregion

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        HandleMovement();
        HandleJumpBuffering();  // Verifica si hay un salto en buffer
        //HandleRotation();
    }
    /*
    #region EdgeDetector
    private bool DetectEdge()
    {
        // Si ya estamos en el suelo, no detectamos bordes
        if (isGrounded) return false;
        // Realizar raycasts desde cada punto
        for (int i = 0; i < edgeRaycastOrigins.Length; i++)
        {
            Vector3 origin = edgeRaycastOrigins[i].position;
            if (Physics.Raycast(origin, -Vector3.up, edgeRayLength, groundLayer))
            {
                // Si detectamos un borde, corregir la posición
                CorrectPosition(i);
                return true;  // Está cerca de un borde
            }
        }
        return false;
    }

    private void CorrectPosition(int edgeIndex)
    {
        // Determinar hacia dónde mover al jugador según el borde detectado
        Vector3 correctionDirection = Vector3.zero;

        switch (edgeIndex)
        {
            case 0: // Borde derecho
                correctionDirection = Vector3.right;
                break;
            case 1: // Borde izquierdo
                correctionDirection = Vector3.left;
                break;
            case 2: // Borde frontal
                correctionDirection = Vector3.forward;
                break;
            case 3: // Borde trasero
                correctionDirection = Vector3.back;
                break;
        }

        // Mover al jugador en la dirección del borde con una interpolación suave
        Vector3 targetPosition = transform.position + correctionDirection * 0.5f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, edgeCorrectionSpeed * Time.deltaTime);
        // Verificar nuevamente si el jugador está sobre el suelo después de la corrección
        if (!Physics.Raycast(transform.position, -Vector3.up, rayCastHeightOffset + 0.1f, groundLayer))
        {
            // Si después de la corrección no está en el suelo, desactiva edge detection
            isNearEdge = false;
        }
    }
    #endregion
    */
}
