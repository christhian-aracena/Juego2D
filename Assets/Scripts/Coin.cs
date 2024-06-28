using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float moveDuration = 1f; // Duraci�n del movimiento en segundos
    public RectTransform coinIconTransform; // Transform del �cono de moneda en la UI



    private Vector3 targetPosition;

    void Start()
    {
        // Encuentra el �cono de la moneda en la escena
        if (coinIconTransform == null)
        {
            GameObject coinIcon = GameObject.FindGameObjectWithTag("CoinIcon");
            if (coinIcon != null)
            {
                coinIconTransform = coinIcon.GetComponent<RectTransform>();
            }
        }


    }
      public void MoveToIconAndDestroy()
    {
        if (coinIconTransform != null)
        {
            StartCoroutine(MoveAndDestroy());
        }
        else
        {
            // Si no se encuentra el �cono de moneda, destruir la moneda de inmediato
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            // Actualizar la posici�n objetivo en cada frame
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, coinIconTransform.position);
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
            targetPosition.z = 0; // Asegura que la posici�n z sea 0 para 2D

            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que la moneda llegue a la posici�n objetivo exacta
        transform.position = targetPosition;

        // Destruir la moneda despu�s de la animaci�n
        Destroy(gameObject);
    } 
}
