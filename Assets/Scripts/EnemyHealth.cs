using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;           // Salud m�xima del enemigo
    public float damageDuration = 0.5f; // Duraci�n del cambio de color al recibir da�o
    public float deathBlinkDuration = 1f; // Duraci�n total del parpadeo antes de desaparecer
    public float blinkInterval = 0.1f;  // Intervalo de parpadeo

    public int currentHealth;          // Salud actual del enemigo
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Collider2D enemyCollider;   // Collider del enemigo

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>(); // Obtener el Collider2D del enemigo

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageEffect());
        }
    }

    private IEnumerator DamageEffect()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(damageDuration);
            spriteRenderer.color = originalColor;
        }
    }

    public void Die()
    {
        StartCoroutine(DeathBlinkingEffect());
    }

    private IEnumerator DeathBlinkingEffect()
    {
        float elapsedTime = 0f;
        bool colliderActiveState = enemyCollider.enabled; // Guardar el estado actual del collider

        // Desactivar el collider del enemigo
        enemyCollider.enabled = false;

        while (elapsedTime < deathBlinkDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Alternar visibilidad
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        spriteRenderer.enabled = true; // Asegurarse de que el sprite est� visible al final

        // Activar de nuevo el collider del enemigo
        enemyCollider.enabled = colliderActiveState;

        // Obtener referencia al script EnemyMovement y llamar su m�todo Die
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.Die();
        }
        else
        {
            Destroy(gameObject); // Eliminar enemigo de la escena si no tiene el script EnemyMovement
        }
    }
}
