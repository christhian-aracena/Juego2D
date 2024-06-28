using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject particlePrefab;  // El prefab de partículas
    public float startX = -50f;        // La posición X inicial
    public float endX = 50f;           // La posición X final
    public float interval = 10f;       // El intervalo entre instancias de partículas
    public float spawnY = 0f;          // La posición Y donde se instanciarán las partículas

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
