using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public GameObject devil;
    private Portal portal;
    private bool isPortalUnlocked = false;
    private float checkInterval = 1f;
    private int consecutiveNoEnemiesCount = 0;
    private int requiredNoEnemiesChecks = 3;

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
            consecutiveNoEnemiesCount++;

            if (consecutiveNoEnemiesCount >= requiredNoEnemiesChecks && !isPortalUnlocked)
            {
                AllEnemiesDefeated();
            }
        }
        else
        {
            consecutiveNoEnemiesCount = 0;

            if (isPortalUnlocked)
            {
                LockPortal();
            }

            if (devil.activeSelf)
            {
                devil.SetActive(false);
            }
        }
    }

    void AllEnemiesDefeated()
    {
        portal.UnlockPortal();
        isPortalUnlocked = true;

        if (SceneManager.GetActiveScene().buildIndex == 4 && !devil.activeSelf)
        {
            devil.SetActive(true);
        }
    }

    void LockPortal()
    {
        portal.LockPortal();
        isPortalUnlocked = false;
    }
}


