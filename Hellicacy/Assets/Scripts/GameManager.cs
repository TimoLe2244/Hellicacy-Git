using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerChoice = 0;
    private bool betterEffectActive = false;
    public int maxHealth = 100;
    public int currentHealth;
    public int maxEnergy = 100;
    public int currentEnergy;
    public int defaultLives = 1;
    public int currentLives;
    private bool statReset;
    private bool isImmune = false;
    private float immunityDuration = 1f;
    public GameObject player;
    private Vector3 lastPlayerPosition;
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
        currentLives = defaultLives;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ActivateBetterEffect()
    {
        betterEffectActive = true;
    }

    public bool IsBetterEffectActive()
    {
        return betterEffectActive;
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
            ResetPlayerTemporaryStats();
            EnablePlayerFeatures();
            ResetBetterEffect();
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

    public void ChangeLives(int amount)
    {
        currentLives += amount;
    }

    private void ResetPlayerTemporaryStats()
    {
        currentHealth = maxHealth;
        currentEnergy = 0;
        currentLives = defaultLives;
    }

    private void Die()
    {
        if (currentLives > 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.GetComponent<Player>().ChangeLives(-1);
            }
            currentHealth = Mathf.CeilToInt(maxHealth * 0.8f);
            lastPlayerPosition = player.transform.position;
            RespawnPlayer();
        }
        else
        {
            SceneManager.LoadScene(8);
        }
    }

    private void RespawnPlayer()
    {
        currentHealth = Mathf.CeilToInt(maxHealth * 0.8f);
        player.transform.position = lastPlayerPosition;
        StartCoroutine(GrantTemporaryImmunity());
        StartCoroutine(FlashSprites());
    }

    [SerializeField] private SpriteRenderer[] bodyParts;

    private IEnumerator FlashSprites()
    {
        int flashes = 4;
        float flashDuration = 0.1f;

        for (int i = 0; i < flashes; i++)
        {
            foreach (SpriteRenderer sr in bodyParts)
            {
                sr.color = Color.red;
            }
            yield return new WaitForSeconds(flashDuration);

            foreach (SpriteRenderer sr in bodyParts)
            {
                sr.color = Color.clear;
            }
            yield return new WaitForSeconds(flashDuration);
        }

        foreach (SpriteRenderer sr in bodyParts)
        {
            sr.color = Color.white;
        }
    }

    private IEnumerator GrantTemporaryImmunity()
    {
        isImmune = true;
        yield return new WaitForSeconds(immunityDuration);
        isImmune = false;
    }

    public void ResetBetterEffect()
    {
        betterEffectActive = false;
    }

}
