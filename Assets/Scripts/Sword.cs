using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player1"))
        {
            Personaje playerCharacter = collision.gameObject.GetComponent<Personaje>();
            if (playerCharacter != null)
            {
                playerCharacter.setLifeDamage(); // El jugador recibe 1 de daño
            }
        }
    }
}
