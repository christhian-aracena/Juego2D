using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public float moveDuration = 1f; // Duración del movimiento en segundos
    public RectTransform heartIconTransform; // Transform del ícono de corazón en la UI

    private Vector3 targetPosition;

    void Start()
    {
        // Encuentra el ícono de corazón en la escena
        if (heartIconTransform == null)
        {
            GameObject heartIcon = GameObject.FindGameObjectWithTag("HeartIcon");
            if (heartIcon != null)
            {
                heartIconTransform = heartIcon.GetComponent<RectTransform>();
            }
        }
    }

    public void MoveToIconAndDestroy()
    {
        if (heartIconTransform != null)
        {
            StartCoroutine(MoveAndDestroy());
        }
        else
        {
            // Si no se encuentra el ícono de corazón, destruir el corazón de inmediato
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            // Actualizar la posición objetivo en cada frame
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, heartIconTransform.position);
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
            targetPosition.z = 0; // Asegura que la posición z sea 0 para 2D

            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que el corazón llegue a la posición objetivo exacta
        transform.position = targetPosition;

        // Destruir el corazón después de la animación
        Destroy(gameObject);
    }
}
