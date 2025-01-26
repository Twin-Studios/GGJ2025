using System;
using System.Collections;
using Assets.Scenes.Juan;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private float spawnRate = 1f;

    private Coroutine _spawningRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindFirstObjectByType<PlayerManager>().GameStarted.AddListener(OnGameStarted);
    }

    private void OnGameStarted()
    {
        if (_spawningRoutine == null)
        {
            _spawningRoutine = StartCoroutine(SpawnEnemies());
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
