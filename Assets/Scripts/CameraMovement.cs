using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;       // Referencia al transform del jugador
    public float speed = 5f;       // Velocidad de movimiento de la cámara
    public float startingPoint = 0f; // Punto inicial de la cámara en el eje X
    public float maxX = 100f;      // Límite máximo a la derecha
    public float offsetY = 2f;     // Desplazamiento en Y respecto al jugador

    void Update()
    {
        if (player != null)
        {
            float targetX = Mathf.Clamp(player.position.x, startingPoint, maxX);
            float targetY = player.position.y + offsetY;
            Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

            // Suaviza el movimiento de la cámara
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
