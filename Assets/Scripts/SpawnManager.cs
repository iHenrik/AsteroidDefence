using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private const float SPAWN_MARGIN = 2f;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject asteroid;

    [SerializeField]
    private float spawnRate = 5f;

    private float currentSpawnRate = 0f;
    private float spawnRateDecrease = 0.2f;
    private Vector3 stageDimensions;
    private int lastSpawnBorder = 0;

    public void Start()
    {
        stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }
    
    public void StartSpawning()
    {
        currentSpawnRate = spawnRate;
        StartCoroutine(SpawnAsteroid());
    }
    

    IEnumerator SpawnAsteroid()
    {
        while (gameManager.CurrentGameState == GameManager.GameState.Game)
        {
            Vector3 spawnPosition = GetAsteroidSpawnPosition();
            
            GameObject.Instantiate(asteroid, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(currentSpawnRate);

            currentSpawnRate -= spawnRateDecrease;

            if (currentSpawnRate < 1)
            {
                currentSpawnRate = 1f;
            }
        }

        StopCoroutine(SpawnAsteroid());
    }

    private Vector3 GetAsteroidSpawnPosition()
    {
        if (stageDimensions.x == 0 && stageDimensions.y == 0)
        {
            Debug.LogWarning("Stage dimensions are zero.");
        }

        int spawnBorder = Random.Range(1, 5);
        while (lastSpawnBorder == spawnBorder)
        {
            spawnBorder = Random.Range(1, 5);
        }
        lastSpawnBorder = spawnBorder;

        Vector3 spawnPosition = new Vector3();

        if (spawnBorder == 1) //Negative X-border
        {
            spawnPosition.x = (stageDimensions.x + SPAWN_MARGIN) * -1;
            spawnPosition.y = Random.Range((stageDimensions.y * -1), stageDimensions.y);
        }
        else if (spawnBorder == 2) //Positive Y-border
        {
            spawnPosition.y = stageDimensions.y + SPAWN_MARGIN;
            spawnPosition.x = Random.Range(stageDimensions.x, stageDimensions.x);
        }
        else if (spawnBorder == 3) //Positive X-border
        {
            spawnPosition.x = (stageDimensions.x + SPAWN_MARGIN);
            spawnPosition.y = Random.Range(stageDimensions.y, stageDimensions.y);
        }
        else //Negative Y-border
        {
            spawnPosition.y = (stageDimensions.y + SPAWN_MARGIN) * -1;
            spawnPosition.x = Random.Range((stageDimensions.x * -1), stageDimensions.x);
        }

        return spawnPosition;
    }
}
