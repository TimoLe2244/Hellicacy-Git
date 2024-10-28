using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool isUnlocked = false;
    public int sceneBuildIndex;

    public void UnlockPortal()
    {
        isUnlocked = true;
        GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUnlocked && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
