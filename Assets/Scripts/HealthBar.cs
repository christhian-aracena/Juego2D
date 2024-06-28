using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill; // Imagen de la barra de vida
    public EnemyHealth life; // Referencia al script SwordLord para obtener la vida del enemigo

    void Start()
    {
        // Buscar el componente SwordLord en el mismo objeto o en su padre
         life = GetComponentInParent<EnemyHealth>();

        // Obtener referencia al componente Image de la barra de vida
        healthBarFill = GetComponent<Image>();

        // Verificar las referencias
        if (life == null)
        {
            Debug.LogWarning("No se encontró el componente SwordLord adjunto a este objeto o a su padre.");
        }

        if (healthBarFill == null)
        {
            Debug.LogWarning("No se encontró el componente Image adjunto a este objeto.");
        }
    }

    void Update()
    {
        if (life != null && healthBarFill != null)
        {
            // Actualizar fill amount basado en la salud actual y máxima del enemigo
            float fillAmount = (float)life.currentHealth / (float)life.maxHealth;
            healthBarFill.fillAmount = fillAmount;

            // Debug para verificar la actualización
            Debug.Log("Fill Amount: " + fillAmount);
        }
        else
        {
            Debug.LogWarning("Las referencias SwordLord o healthBarFill son nulas en EnemyHealthBar.");
        }
    }
}
