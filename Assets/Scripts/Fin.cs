using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fin : MonoBehaviour
{
    public string nextSceneName = "GameOver"; // Nombre de la escena a la que quieres cambiar
    public float delayBeforeSceneChange = 2f; // Tiempo de espera antes de cambiar de escena
    public float setHeight = 23.4f; // Altura deseada para detener al personaje
    private Animator playerAnimator; // Referencia al Animator del jugador
    public Collider2D colliderToActivate; // Collider que se activará al colisionar con "Fin"

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            Debug.Log("Esta colisionando con Fin");

            // Obtener el Animator del jugador
            playerAnimator = other.gameObject.GetComponent<Animator>();

            if (playerAnimator != null)
            {
                // Ajustar la posición del personaje para evitar que se mueva más allá del suelo
                Vector3 newPosition = other.transform.position;
                newPosition.y = setHeight; // Ajustar la altura del personaje explícitamente
                other.transform.position = newPosition;

                Debug.Log("Nueva posición del personaje: " + newPosition.y);

                // Iniciar la animación
                playerAnimator.SetTrigger("Finish"); // Asegúrate de tener un trigger llamado "Finish" en tu Animator
            }

            // Activar el collider del otro objeto si está asignado
            if (colliderToActivate != null)
            {
                colliderToActivate.gameObject.SetActive(true);
            }

            // Iniciar la corrutina para cambiar de escena después de un retraso
            StartCoroutine(ChangeSceneAfterDelay());
        }
    }

    private IEnumerator ChangeSceneAfterDelay()
    {
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // Cambiar de escena
        SceneManager.LoadScene(nextSceneName);
    }
}
