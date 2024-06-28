using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    public Transform target; // Referencia al objeto que seguirá el canvas (enemigo en este caso)

    void Update()
    {
        if (target != null)
        {
            // Mantener la posición del canvas igual a la posición del enemigo
            transform.position = target.position;
        }
    }
}
