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
        if (scene.name == "restaurant")
        {
            statReset = false;
            ResetPlayerTemporaryStats();
        }
        else
        {
            statReset = false;
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




