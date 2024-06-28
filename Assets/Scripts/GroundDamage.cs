using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDamage : MonoBehaviour
{



    public void OnCollisionEnter2D(Collision2D other)
    {
        Personaje characterLife = other.gameObject.GetComponent<Personaje>();
        if (other.gameObject.CompareTag("Player1"))
        {
            Debug.Log("Esta colisionando con Player1");
            characterLife.life = 0;
            characterLife.UpdateHearts();
            characterLife.Die();

        }
    }
}