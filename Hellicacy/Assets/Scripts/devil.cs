using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class devil : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject nextButton;
    public GameObject contractPanel;

    public Text dialogueText;
    public string[] dialogue;
    private int index;

    public float wordSpeed;
    public bool playerIsClose;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if(dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else{
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if(dialogueText.text == dialogue[index])
        {
            nextButton.SetActive(true);
        }
    }


    public void NextLine()
    {
        nextButton.SetActive(false);

        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else{
            zeroText();
            OpenContractMenu();
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }

    private void OpenContractMenu()
    {
        contractPanel.SetActive(true);
    }

    public void OnChoice1Selected()
    {
        // Add any logic specific to choice 1
        CloseContractMenu();
    }

    public void OnChoice2Selected()
    {
        // Add any logic specific to choice 2
        CloseContractMenu();
    }

    public void OnChoice3Selected()
    {
        // Add any logic specific to choice 3
        CloseContractMenu();
    }

    private void CloseContractMenu()
    {
        if (contractPanel != null)
        {
            contractPanel.SetActive(false);
        }
    }
}
