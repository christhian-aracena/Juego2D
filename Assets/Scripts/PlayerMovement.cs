using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // Velocidad de movimiento horizontal
    public float jumpForce = 10f;      // Fuerza del salto
    public Transform groundCheck;      // Transform para verificar si est� en el suelo
    public float groundCheckRadius = 0.3f;  // Radio de verificaci�n del suelo
    public LayerMask groundLayer;      // Capa del suelo
    public float attackDuration = 0.5f; // Duraci�n del ataque en segundos
    public Transform wallCheck;        // Transform para verificar si est� tocando una pared
    public float wallCheckDistance = 0.2f; // Distancia para verificar la colisi�n con la pared
    public LayerMask wallLayer;        // Capa de las paredes
    public float maxSlopeAngle = 45f;  // �ngulo m�ximo de pendiente permitido

    private Rigidbody2D rb;
    private Animator animator;         // Referencia al componente Animator
    private bool isGrounded;
    private bool isAttacking;          // Booleano para controlar la animaci�n de ataque
    private bool facingRight = true;   // Variable para controlar la direcci�n del personaje
    private float jumpCooldown = 0.9f; // Tiempo de cooldown para evitar detecci�n inmediata
    private float jumpCooldownTimer = 0f; // Temporizador para el cooldown
    private float attackTimer = 0f;    // Temporizador para la duraci�n del ataque
    private bool isTouchingWall;       // Variable para verificar si est� tocando una pared

    public PhysicsMaterial2D noFriction; // Material sin fricci�n
    public PhysicsMaterial2D withFriction; // Material con fricci�n

    public BoxCollider2D attackCollider; // Referencia al BoxCollider2D del ataque

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obt�n el componente Animator
        withFriction = rb.sharedMaterial; // Asume que el material por defecto tiene fricci�n
        attackCollider.enabled = false; // Aseg�rate de que el collider de ataque est� desactivado al inicio
    }

    void Update()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Verificar si est� en el suelo
        bool wasGrounded = isGrounded;
        jumpCooldownTimer -= Time.deltaTime;
        if (jumpCooldownTimer <= 0f)
        {
            isGrounded = IsGrounded();
        }

        // Verificar si est� tocando una pared
        isTouchingWall = IsTouchingWall();

        // Saltar
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Aplicar la fuerza de salto
            isGrounded = false; // Establecer isGrounded a false al saltar
            jumpCooldownTimer = jumpCooldown; // Reiniciar el temporizador de cooldown
        }

        // Atacar
        if (!isAttacking && Input.GetMouseButtonDown(0)) // Asegurarse de que no se est� atacando ya
        {
            isAttacking = true;
            attackTimer = 0f; // Reiniciar el temporizador de duraci�n del ataque
            animator.SetBool("isAttacking", true); // Activar la animaci�n de ataque
            attackCollider.enabled = true; // Activar el collider de ataque
        }

        // Controlar la duraci�n del ataque
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                animator.SetBool("isAttacking", false); // Desactivar la animaci�n de ataque
                attackCollider.enabled = false; // Desactivar el collider de ataque
            }
        }

        // Voltear el personaje si es necesario
        if (moveInput > 0 && !facingRight || moveInput < 0 && facingRight)
        {
            Flip();
        }

        // Evitar que el jugador se quede pegado a las paredes
        if (isTouchingWall && !isGrounded && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (isTouchingWall && Mathf.Abs(moveInput) > 0.1f)
        {
            float direction = facingRight ? -1 : 1;
            rb.AddForce(new Vector2(direction * 2f, 0), ForceMode2D.Impulse);
        }

        // Verificar si est� en una pendiente muy inclinada
        CheckForSlopes();

        // Actualizar el estado de la animaci�n
        UpdateAnimation(moveInput, wasGrounded);
    }

    private void HandleWallFriction()
    {
        if (isTouchingWall && !isGrounded && Mathf.Abs(rb.velocity.x) > 0)
        {
            rb.sharedMaterial = noFriction;
        }
        else
        {
            rb.sharedMaterial = withFriction;
        }
    }

    private void CheckForSlopes()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius + 0.1f, groundLayer);
        if (hit.collider != null)
        {
            Vector2 normal = hit.normal;
            float angle = Vector2.Angle(normal, Vector2.up);

            if (angle > maxSlopeAngle) // Ajusta el �ngulo seg�n sea necesario
            {
                // Empujar al personaje lejos de la pendiente
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y > 0 ? rb.velocity.y : 0); // Resetear la velocidad vertical si va hacia arriba
            }
        }
    }

    private void UpdateAnimation(float moveInput, bool wasGrounded)
    {
        // Si el movimiento horizontal es diferente de cero, el jugador est� corriendo
        bool isRunning = Mathf.Abs(moveInput) > 0.01f;
        animator.SetBool("isRunning", isRunning);

        // Actualizar el par�metro "isJumping" en el Animator
        animator.SetBool("isJumping", !isGrounded);

        // Actualizar el par�metro "isGrounded" en el Animator
        animator.SetBool("isGrounded", isGrounded);

        // Actualizar el par�metro "isAttacking" en el Animator
        animator.SetBool("isAttacking", isAttacking);

        // Mensajes de depuraci�n
        Debug.Log("isRunning: " + isRunning);
        Debug.Log("isGrounded: " + isGrounded);
        Debug.Log("isJumping: " + !isGrounded);
        Debug.Log("isAttacking: " + isAttacking);
        Debug.Log("isTouchingWall: " + isTouchingWall);
    }

    private bool IsGrounded()
    {
        // Usar OverlapCircle para detectar colisiones con el suelo
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return collider != null;
    }

    private bool IsTouchingWall()
    {
        // Usar Raycast para detectar colisiones con las paredes
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, facingRight ? Vector2.right : Vector2.left, wallCheckDistance, wallLayer);
        return hit.collider != null;
    }

    private void Flip()
    {
        // Ajustar la posici�n del personaje antes de voltear si est� tocando una pared
        if (isTouchingWall)
        {
            // Desplazar al personaje hacia el centro de la plataforma antes de voltear
            float direction = facingRight ? -1 : 1; // Si est� mirando a la derecha, mover hacia la izquierda y viceversa
            transform.position += new Vector3(direction * -5f, 15, 15); // Ajusta el valor 0.5f seg�n sea necesario
        }

        // Cambiar la direcci�n en la que est� mirando el personaje
        facingRight = !facingRight;

        // Ajustar la escala X del personaje para voltearlo
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //el da�o visual del enemigho
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (facingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
        }
    }
}