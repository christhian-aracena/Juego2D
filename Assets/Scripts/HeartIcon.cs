using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartIcon : MonoBehaviour
{
    public List<Animator> heartAnimators; // Lista de animators de todos los corazones en la UI

    void Start()
    {
        // Inicializar la lista de animators si no está configurada desde el inspector
        if (heartAnimators == null || heartAnimators.Count == 0)
        {
            heartAnimators = new List<Animator>();
            foreach (Transform child in transform)
            {
                Animator animator = child.GetComponent<Animator>();
                if (animator != null)
                {
                    heartAnimators.Add(animator);
                }
            }
        }
    }

    public void StartAllHeartAnimations()
    {
        foreach (Animator animator in heartAnimators)
        {
            animator.Play("Rotate", -1, 0f); // Iniciar la animación desde el inicio
        }
    }
}
