using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunaFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Referencia al transform de la cámara

    void Update()
    {
        // La posición y de la luna se mantiene fija, mientras que la posición x sigue a la cámara
        transform.position = new Vector3(cameraTransform.position.x, transform.position.y, transform.position.z);
    }
}
