using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveDistance = 2f;     // Distancia que el enemigo se mover� hacia arriba y hacia abajo
    public float moveDuration = 1f;     // Duraci�n del movimiento de un punto a otro

    private Vector3 initialPosition;    // Posici�n inicial del enemigo
    private Vector3 targetPosition;     // Posici�n objetivo
    private bool movingUp = true;       // Indica si el enemigo est� movi�ndose hacia arriba

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;
        StartCoroutine(MoveEnemyCoroutine());
    }

    IEnumerator MoveEnemyCoroutine()
    {
        while (true)
        {
            Vector3 startPosition = movingUp ? initialPosition : targetPosition;
            Vector3 endPosition = movingUp ? targetPosition : initialPosition;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition;
            movingUp = !movingUp;
            yield return null;
        }
    }

    // M�todo para manejar la muerte del enemigo
    public void Die()
    {
        // Destruir el objeto del enemigo original
        Destroy(gameObject);
    }

    // M�todo para detectar colisiones
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aqu� podr�as manejar el da�o al jugador si es necesario
            // Llamar al m�todo Die() cuando el enemigo muere
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Die();
            }
        }
    }
}
