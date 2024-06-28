using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLord : MonoBehaviour
{
    public int maxHealth = 4;
    public int health;
    public float detectionRadius = 10f;
    public float attackRadius = 1.5f;
    public float speed = 2f;
    public float attackCooldown = 1f;
    public float attackDuration = 0.5f;

    private Transform player;
    private Animator animator;
    private bool isDead = false;
    private bool isAttacking = false; // Variable para manejar el estado de ataque
    private float lastAttackTime = 0f;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;
    private GameObject coin;

    public BoxCollider2D swordCollider; // Collider de la espada
    public GameObject enemyCanvas; // Referencia al canvas que contiene la barra de vida
    public GameObject experienceCoin; // Prefab de la moneda de experiencia
    public Vector3 coinScale = new Vector3(1f, 1f, 1f); // Tamaño de la moneda de experiencia

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player1").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale; // Almacenar la escala original
        health = maxHealth; // Inicializar la vida

        if (swordCollider != null)
        {
            swordCollider.enabled = false; // Desactivar el collider de la espada al inicio
        }

        if (enemyCanvas == null)
        {
            Debug.LogWarning("EnemyCanvas no asignado en SwordLord.");
        }
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Orientar hacia el personaje manteniendo la escala original
        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRadius && !isAttacking)
        {
            // Cambiar a animación de correr
            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);

            // Mover hacia el personaje
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceToPlayer <= attackRadius && Time.time > lastAttackTime + attackCooldown)
        {
            // Cambiar a animación de ataque
            animator.SetBool("isRunning", false);
            animator.SetTrigger("isAttacking");
            lastAttackTime = Time.time;
            StartCoroutine(AttackPlayer());
        }
        else if (!isAttacking)
        {
            // Cambiar a animación de idle
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", true);
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        if (swordCollider != null)
        {
            swordCollider.enabled = true; // Activar el collider de la espada
        }
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        if (swordCollider != null)
        {
            swordCollider.enabled = false; // Desactivar el collider de la espada
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Die();
        }

        // Debug para verificar la salud actual
        Debug.Log("Current Health: " + health);
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        // Desactivar el EnemyCanvas cuando el enemigo muere
        if (enemyCanvas != null)
        {
            enemyCanvas.SetActive(false);
        }

        Debug.Log("Enemy has died. Disabling health bar and activating experience coin.");

        // Iniciar la corrutina para manejar la moneda de experiencia
        StartCoroutine(HandleExperienceCoin());
    }

    private IEnumerator HandleExperienceCoin()
    {
        // Esperar 2 segundos
        yield return new WaitForSeconds(2f);

        // Instanciar la moneda de experiencia
        GameObject coin = Instantiate(experienceCoin, transform.position, Quaternion.identity);

        // Ajustar la escala de la moneda de experiencia
        coin.transform.localScale = coinScale;

        // Agregar Rigidbody2D si no está presente
        Rigidbody2D rb2D = coin.GetComponent<Rigidbody2D>();

        // Aplicar fuerza hacia arriba para simular el salto
        rb2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        // Iniciar la corrutina para hacer parpadear y destruir la moneda
        StartCoroutine(BlinkAndDestroyCoin(coin));

        // Destruir el objeto después de la creación de la moneda
        Destroy(gameObject);
    }

    private IEnumerator BlinkAndDestroyCoin(GameObject coin)
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // Empezar a hacer parpadear la moneda
        SpriteRenderer sr = coin.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            for (int i = 0; i < 10; i++)
            {
                sr.enabled = !sr.enabled;
                yield return new WaitForSeconds(0.1f);
            }
        }

        // Destruir la moneda después de 2 segundos
        Destroy(coin);
    }

}
