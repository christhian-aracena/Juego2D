using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceCoinController : MonoBehaviour
{
    public float moveDuration = 1f; // Duración del movimiento en segundos
    public float jumpForce = 5f;    // Fuerza de salto al aparecer
    public float blinkStartDelay = 5f; // Tiempo antes de comenzar a parpadear
    public float blinkDuration = 3f; // Duración del parpadeo antes de desaparecer
    public float blinkInterval = 0.1f; // Intervalo de parpadeo en segundos

    private RectTransform coinIconTransform; // Transform del ícono de moneda en la UI
    private Rigidbody2D rb; // Rigidbody2D de la moneda
    private SpriteRenderer spriteRenderer; // SpriteRenderer de la moneda

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Aplicar una fuerza hacia arriba al iniciar para simular el salto
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Iniciar la rutina de parpadeo y desaparición
        StartCoroutine(BlinkAndDisappear());
    }

    // Método para iniciar el movimiento hacia el icono de moneda
    public void MoveToIcon(RectTransform iconTransform)
    {
        coinIconTransform = iconTransform;
        StartCoroutine(MoveAndDestroy());
    }

    private IEnumerator MoveAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        // Desactivar el Rigidbody2D mientras se mueve hacia el icono
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        while (elapsedTime < moveDuration)
        {
            // Calcular posición objetivo en cada frame
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, coinIconTransform.position);
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
            targetPosition.z = 0; // Asegurar que la posición z sea 0 para 2D

            // Interpolar posición hacia la posición objetivo
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / moveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que la moneda llegue a la posición objetivo exacta
        transform.position = coinIconTransform.position;

        // Destruir la moneda después de la animación de movimiento
        Destroy(gameObject);
    }

    private IEnumerator BlinkAndDisappear()
    {
        // Esperar antes de comenzar a parpadear
        yield return new WaitForSeconds(blinkStartDelay);

        float blinkElapsedTime = 0f;

        while (blinkElapsedTime < blinkDuration)
        {
            // Alternar entre visible e invisible
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);

            blinkElapsedTime += blinkInterval;
        }

        // Asegurarse de que la moneda sea visible al final del parpadeo
        spriteRenderer.enabled = true;

        // Destruir la moneda después del parpadeo
        Destroy(gameObject);
    }
}
