using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool isUnlocked = false;
    private bool isLocked = true;
    public int sceneBuildIndex;
    public AudioSource portalSound;
    private bool playerOnPortal = false;

    public void UnlockPortal()
    {
        isUnlocked = true;
        isLocked = false;
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void LockPortal()
    {
        isLocked = true;
        isUnlocked = false;
        GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUnlocked && other.CompareTag("Player"))
        {
            if (!playerOnPortal)
            {
                playerOnPortal = true;
                portalSound.Play();
                StartCoroutine(WaitBeforeSceneChange());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isUnlocked && other.CompareTag("Player"))
        {
            playerOnPortal = false;
            StopCoroutine(WaitBeforeSceneChange());
        }
    }

    private IEnumerator WaitBeforeSceneChange()
    {
        yield return new WaitForSeconds(1f);
        if (playerOnPortal)
        {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}