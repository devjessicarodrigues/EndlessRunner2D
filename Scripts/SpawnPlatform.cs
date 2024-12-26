using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    public List<GameObject> platforms; 
    public float spawnInterval = 2f; 
    public float minHeight = -1.8f; 
    public float maxHeight = 1.97f; 
    public float maxDistance = 5f; 
    public float minDistance = 2f; 
    public Transform player; 

    private float timer = 0f; 
    private List<GameObject> spawnedPlatforms = new List<GameObject>(); 

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }

        DestroyOffscreenPlatforms();
    }

    void Spawn()
    {
        if (platforms.Count > 0)
        {
            GameObject selectedPlatform = platforms[Random.Range(0, platforms.Count)];

            float spawnX;

            if (spawnedPlatforms.Count > 0)
            {
                GameObject lastPlatform = spawnedPlatforms[spawnedPlatforms.Count - 1];
                spawnX = lastPlatform.transform.position.x + Random.Range(minDistance, maxDistance);
            }
            else
            {
                spawnX = Mathf.Max(player.position.x + Random.Range(minDistance, maxDistance), 39.21f);
            }

            float spawnY = Random.Range(minHeight, maxHeight);

            Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

            GameObject instantiatedPlatform = Instantiate(selectedPlatform, spawnPos, Quaternion.identity);

            spawnedPlatforms.Add(instantiatedPlatform);
        }
    }

    void DestroyOffscreenPlatforms()
    {
        Camera mainCamera = Camera.main;
        float leftLimit = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;

        for (int i = spawnedPlatforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = spawnedPlatforms[i];
            if (platform != null)
            {
                if (platform.transform.position.x < leftLimit)
                {
                    Destroy(platform);
                    spawnedPlatforms.RemoveAt(i); 
                }
            }
        }
    }
}
