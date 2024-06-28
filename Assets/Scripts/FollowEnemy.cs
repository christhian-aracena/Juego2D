using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    public Transform target; // Referencia al objeto que seguir� el canvas (enemigo en este caso)

    void Update()
    {
        if (target != null)
        {
            // Mantener la posici�n del canvas igual a la posici�n del enemigo
            transform.position = target.position;
        }
    }
}
