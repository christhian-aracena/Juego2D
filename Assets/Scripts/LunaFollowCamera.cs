using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Referencia al transform de la c�mara

    void Update()
    {
        // La posici�n y de la luna se mantiene fija, mientras que la posici�n x sigue a la c�mara
        transform.position = new Vector3(cameraTransform.position.x, transform.position.y, transform.position.z);
    }
}
