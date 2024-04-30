using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;



public class ZombieSpawner : MonoBehaviour
{
    [Serializable]
    public struct MinMaxValues<T>
    {
        [SerializeField] public T min;
        [SerializeField] public T max;
    }

    // Start is called before the first frame update
    [SerializeField] private float spawnDistance = 20f;
    [SerializeField] private GameObject zombiePrefab;

    [SerializeField] private ARRaycastManager m_arRaycastManager;
    private List<ARRaycastHit> m_arRaycastHits = new List<ARRaycastHit>();
    [SerializeField] private MinMaxValues<int> enemiesToSpawnPerWave;
    [SerializeField] private MinMaxValues<float> timeBetweenSpawningPerWave;
    [SerializeField] private float timeToGetToFullWave;

    void Start()
    {
        
    }

    private float waveTimer = 0.0f;
    private float totalTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        totalTimer += Time.deltaTime;
        waveTimer += Time.deltaTime;

        float scalar = totalTimer / timeToGetToFullWave;

        float currentWaveTimer = timeBetweenSpawningPerWave.min + ((timeBetweenSpawningPerWave.max - timeBetweenSpawningPerWave.min) * scalar);
        int currentSpawning = enemiesToSpawnPerWave.min + ((enemiesToSpawnPerWave.max - enemiesToSpawnPerWave.min) * (int)scalar);

        if (waveTimer >= currentWaveTimer)
        {
            for (int i = 0; i < currentSpawning; i++)
            {
                SpawnZombie();
            }
            waveTimer = 0;
        }
        
        
    }

    public void SpawnZombie()
    {
        Vector3 playerForward = GameManager.Instance.PlayerTransform.forward;
        int degree = UnityEngine.Random.Range(0, 360) + 1;

        Vector3 newDirection = Quaternion.Euler(0, degree, 0) * playerForward;
        Vector3 spawnPos = newDirection * spawnDistance;
        spawnPos.y = GameManager.Instance.PlayerTransform.position.y - 1.25f;
        
        Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
    }
}
