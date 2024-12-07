using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxHealth = 100;
    public int currentHealth;
    public int maxEnergy = 100;
    public int currentEnergy;
    private bool statReset;

    public GameObject player;
    private MonoBehaviour[] playerScripts;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = 0;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player != null)
        {
            playerScripts = player.GetComponents<MonoBehaviour>();
        }

        if (scene.name == "restaurant")
        {
            statReset = false;
            ResetPlayerTemporaryStats();
            EnablePlayerFeatures();
        }
        else if (scene.name == "character")
        {
            DisablePlayerFeatures();
        }
        else
        {
            statReset = false;
            EnablePlayerFeatures();
        }
    }

    private void DisablePlayerFeatures()
    {
        if (player != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                script.enabled = false;
            }

            GameObject attackpoint = player.transform.Find("attackpoint")?.gameObject;
            if (attackpoint != null)
            {
                attackpoint.SetActive(false);
            }

            Debug.Log("Player features disabled in character scene.");
        }
        else
        {
            Debug.LogWarning("Player object is not assigned.");
        }
    }

    private void EnablePlayerFeatures()
    {
        if (player != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                script.enabled = true;
            }

            GameObject attackpoint = player.transform.Find("attackpoint")?.gameObject;
            if (attackpoint != null)
            {
                attackpoint.SetActive(true);
            }

            Debug.Log("Player features enabled.");
        }
        else
        {
            Debug.LogWarning("Player object is not assigned.");
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ChangeEnergy(int amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    private void ResetPlayerTemporaryStats()
    {
        currentHealth = maxHealth;
        currentEnergy = 0;
        Debug.Log("Player health reset to full!");
        statReset = true;
    }

    private void Die()
    {
        Debug.Log("Player has died! Showing death screen...");
        SceneManager.LoadScene(8);
    }
}





