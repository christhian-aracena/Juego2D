using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float moveDistance = 5f;  // La distancia que queremos mover la plataforma
    public float moveDuration = 2f;  // El tiempo que queremos que dure el movimiento

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isMoving = false; // Indica si la plataforma está en movimiento

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;
    }

    public void MovePlatform()
    {
        if (!isMoving)
        {
            StartCoroutine(MovePlatformCoroutine());
        }
    }

    IEnumerator MovePlatformCoroutine()
    {
        isMoving = true;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float fracComplete = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, fracComplete);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(5f); // Esperar 5 segundos en la posición superior

        elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            float fracComplete = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(targetPosition, initialPosition, fracComplete);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPosition; // Asegurar que la posición final sea exacta
        isMoving = false;
    }
}
