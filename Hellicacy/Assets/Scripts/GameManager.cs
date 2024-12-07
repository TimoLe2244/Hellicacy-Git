using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxHealth = 100;
    public int currentHealth;
    public int maxEnergy = 100;
    public int currentEnergy;
    public int lives = 2;
    private bool statReset;
    private bool isImmune = false;
    private float immunityDuration = 1f;

    public GameObject player;
    private Vector3 lastPlayerPosition;
    private MonoBehaviour[] playerScripts;

    public Image life1;
    public Image life2;
    public Image life3;

    public Color liveColor = Color.white;
    public Color deadColor = Color.black;

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
        UpdateLivesUI();
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

        life1 = GameObject.Find("Life1Image")?.GetComponent<Image>();
        life2 = GameObject.Find("Life2Image")?.GetComponent<Image>();
        life3 = GameObject.Find("Life3Image")?.GetComponent<Image>();

        if (scene.name == "restaurant")
        {
            ResetPlayerTemporaryStats();
            EnablePlayerFeatures();
        }
        else if (scene.name == "character")
        {
            DisablePlayerFeatures();
        }
        else
        {
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
        if (!isImmune)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            
            if (currentHealth <= 0)
            {
                Die();
            }
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
    }

    private void Die()
    {
        if (lives > 0)
        {
            lives--;
            UpdateLivesUI();
            lastPlayerPosition = player.transform.position;
            currentHealth = Mathf.CeilToInt(maxHealth * 0.8f);
            Debug.Log("Player died but has " + lives + " lives left.");
            RespawnPlayer();
        }
        else
        {
            Debug.Log("Player has no lives left!");
            SceneManager.LoadScene(8);
        }
    }

    private void RespawnPlayer()
    {
        currentHealth = Mathf.CeilToInt(maxHealth * 0.8f);
        player.transform.position = lastPlayerPosition;
        StartCoroutine(GrantTemporaryImmunity());
        Debug.Log("Player respawned with " + currentHealth + " health.");
    }

    private void UpdateLivesUI()
    {
        life1.color = lives >= 2 ? liveColor : deadColor;
        life2.color = lives >= 3 ? liveColor : deadColor;
        life3.color = lives >= 4 ? liveColor : deadColor;

        Debug.Log("Lives left: " + lives);
    }

    private IEnumerator GrantTemporaryImmunity()
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityDuration);
        isImmune = false;
        Debug.Log("Player is no longer immune.");
    }
}







