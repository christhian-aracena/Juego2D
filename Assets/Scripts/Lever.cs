using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform upPosition;  // Referencia a la posici�n visual de la palanca hacia arriba
    public Transform downPosition;  // Referencia a la posici�n visual de la palanca hacia abajo
    public PlatformMovement platform;  // Referencia a la plataforma
    public float timerLever;

    private bool isDown;  // Indica si la palanca est� en la posici�n hacia abajo

    void Start()
    {
        isDown = false;
        SetLeverState(false);  // Inicia mostrando solo la palanca hacia arriba
    }

    public void Activate()
    {
        if (!isDown)
        {
            isDown = true;
            SetLeverState(isDown);  // Cambia el estado visual de la palanca
            Debug.Log("Palanca activada. Estado actual: Abajo");
            platform.MovePlatform(); // Llama a la corutina para mover la plataforma
            Invoke("ResetPosition", timerLever);  // Llama a ResetPosition despu�s de 5 segundos
        }
    }

    void SetLeverState(bool isDown)
    {
        upPosition.gameObject.SetActive(!isDown);
        downPosition.gameObject.SetActive(isDown);
        Debug.Log("Estado de la palanca establecido. isDown: " + isDown);
    }

    void ResetPosition()
    {
        isDown = false;
        SetLeverState(false);  // Restablece la palanca a la posici�n hacia arriba
        Debug.Log("Palanca restablecida a la posici�n hacia arriba.");
    }


}
