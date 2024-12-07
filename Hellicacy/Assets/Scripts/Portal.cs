using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool isUnlocked = false;
    public int sceneBuildIndex;
    public AudioSource portalSound;  // Reference to the AudioSource for the sound effect
    private bool playerOnPortal = false;

    public void UnlockPortal()
    {
        isUnlocked = true;
        GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUnlocked && other.CompareTag("Player"))
        {
            if (!playerOnPortal)
            {
                playerOnPortal = true;
                portalSound.Play();  // Play the sound effect
                StartCoroutine(WaitBeforeSceneChange());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isUnlocked && other.CompareTag("Player"))
        {
            playerOnPortal = false;  // Reset if the player leaves the portal area
            StopCoroutine(WaitBeforeSceneChange());  // Stop scene change if the player leaves
        }
    }

    private IEnumerator WaitBeforeSceneChange()
    {
        yield return new WaitForSeconds(1f);  // Wait for 1 second
        if (playerOnPortal)
        {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);  // Change scene
        }
    }
}