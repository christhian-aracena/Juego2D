using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBarFill; // Imagen de la barra de vida
    public SwordLord swordLord; // Referencia al script SwordLord para obtener la vida del enemigo

    void Start()
    {
        // Obtener referencia al componente Image de la barra de vida
        healthBarFill = GetComponent<Image>();

        // Verificar la referencia
        if (healthBarFill == null)
        {
            Debug.LogWarning("No se encontró el componente Image adjunto a este objeto.");
        }
    }

    void Update()
    {
        // Siempre que healthBarFill no sea nulo, actualiza el fill amount
        if (healthBarFill != null)
        {
            // Verificar si swordLord es nulo y buscarlo solo una vez al inicio
            if (swordLord == null)
            {
                swordLord = FindObjectOfType<SwordLord>(); // Buscar SwordLord en toda la escena
                if (swordLord == null)
                {
                    Debug.LogWarning("No se encontró el componente SwordLord en la escena.");
                    return;
                }
            }

            // Actualizar fill amount basado en la salud actual y máxima del enemigo
            float fillAmount = (float)swordLord.health / (float)swordLord.maxHealth;
            healthBarFill.fillAmount = fillAmount;

            // Debug para verificar la actualización
            Debug.Log("Fill Amount: " + fillAmount);
        }
        else
        {
            Debug.LogWarning("La referencia healthBarFill es nula en EnemyHealthBar.");
        }
    }
}
