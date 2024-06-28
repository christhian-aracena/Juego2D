using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
public float attackDuration = 0.5f;
    public BoxCollider2D attackCollider;

    private bool isAttacking;

    void Start()
    {
        attackCollider.enabled = false; // Asegurarse de que el collider esté desactivado al inicio
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            isAttacking = true;
            attackCollider.enabled = true; // Activar el collider de ataque
            Invoke("DisableAttackCollider", attackDuration); // Desactivar el collider después de attackDuration segundos
        }
    }

    void DisableAttackCollider()
    {
        isAttacking = false;
        attackCollider.enabled = false; // Desactivar el collider de ataque
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lever"))
        {
            Debug.Log("Colisión detectada con: " + other.name);

            Lever lever = other.GetComponentInParent<Lever>(); // Buscar el componente Lever en el objeto padre

            if (lever != null)
            {
                Debug.Log("Golpeaste una palanca!");
                lever.Activate(); // Activar la palanca
            }
            else
            {
                Debug.Log("Palanca no encontrada en el objeto golpeado.");
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Golpeaste a un enemigo!");
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // Suponiendo que el enemigo tiene un método TakeDamage
            }
        }
        else if (other.CompareTag("Enemy1"))
        {
            Debug.Log("Golpeaste a SwordLord!");
            SwordLord enemy = other.GetComponent<SwordLord>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // Suponiendo que el SwordLord tiene un método TakeDamage
            }
        }
    }
}
