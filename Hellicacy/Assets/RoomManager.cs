using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public string enemyTag = "Enemy";

    private Portal portal;

    void Start()
    {
        portal = FindObjectOfType<Portal>();
    }

    void Update()
    {
        CheckForEnemies();
    }

    void CheckForEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        if (enemies.Length == 0)
        {
            AllEnemiesDefeated();
        }
    }

    void AllEnemiesDefeated()
    {
        portal.UnlockPortal();
    }
}
