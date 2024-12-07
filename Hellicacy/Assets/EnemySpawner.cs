using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject spawnPrefab; 
    [SerializeField] Transform spawnPoint;

    public void Spawn()
    {
        Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
