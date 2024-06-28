using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject particlePrefab;  // El prefab de part�culas
    public float startX = -50f;        // La posici�n X inicial
    public float endX = 50f;           // La posici�n X final
    public float interval = 10f;       // El intervalo entre instancias de part�culas
    public float spawnY = 0f;          // La posici�n Y donde se instanciar�n las part�culas

    void Start()
    {
        SpawnParticles();
    }

    void SpawnParticles()
    {
        if (particlePrefab == null)
        {
            Debug.LogError("Particle prefab is not assigned.");
            return;
        }

        for (float x = startX; x <= endX; x += interval)
        {
            Vector3 spawnPosition = new Vector3(x, spawnY, transform.position.z);
            Instantiate(particlePrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Particle spawned at: " + spawnPosition);
        }
    }
}
