using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pared : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Evitar que el jugador se pegue a la pared ajustando la velocidad vertical a 0
                if (playerRb.velocity.y > 0)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
                }
            }
        }
    }
}
