using UnityEngine;

public class PowerSpawner : MonoBehaviour
{
    [SerializeField] private CollectablePowerUp powerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnRate = 3;

    private GameObject currentSpawn;


    private float currentSpawnRate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpawn == null)
        {
            currentSpawnRate -= Time.deltaTime;
            if (currentSpawnRate <= 0)
            {
                SpawnPower();
                currentSpawnRate = spawnRate;
            }
        }
    }

    void SpawnPower()
    {
        powerPrefab.Type = (PowerUpType)Random.Range(0, 3);
        currentSpawn = Instantiate(powerPrefab.gameObject, spawnPoint.position, Quaternion.identity);
    }
}
