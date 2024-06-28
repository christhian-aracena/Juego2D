using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveDistance = 2f;     // Distancia que el enemigo se moverá hacia arriba y hacia abajo
    public float moveDuration = 1f;     // Duración del movimiento de un punto a otro

    private Vector3 initialPosition;    // Posición inicial del enemigo
    private Vector3 targetPosition;     // Posición objetivo
    private bool movingUp = true;       // Indica si el enemigo está moviéndose hacia arriba

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

    // Método para manejar la muerte del enemigo
    public void Die()
    {
        // Destruir el objeto del enemigo original
        Destroy(gameObject);
    }

    // Método para detectar colisiones
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aquí podrías manejar el daño al jugador si es necesario
            // Llamar al método Die() cuando el enemigo muere
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Die();
            }
        }
    }
}
