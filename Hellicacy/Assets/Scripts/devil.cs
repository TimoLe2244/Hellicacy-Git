using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class devil : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject nextButton;
    public GameObject contractPanel;
    public GameObject choice1Button;
    public GameObject choice2Button;
    public GameObject choice3Button;

    public Text dialogueText;
    public string[] dialogue;
    private int index;
    public GameObject devil1;
    private bool healOnDamageActive = false;

    public float wordSpeed;
    public bool playerIsClose;
    private float intimacyLevel;

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
        Debug.Log("Choice 1 selected: Heal on Damage activated!");
        GameManager.Instance.playerChoice = 1;
        CloseContractMenu();
    }

    public void OnChoice2Selected()
    {
        Debug.Log("Choice 2 selected: Extra life granted!");
        GameManager.Instance.playerChoice = 2;
        
        GameManager.Instance.ChangeLives(1);
        
        CloseContractMenu();
    }

    public void OnChoice3Selected()
    {
        float baseChance = 0.10f;
        float intimacyBonus = 0.10f * intimacyLevel;
        float totalChance = baseChance + intimacyBonus;

        float randomValue = Random.value;

        if (randomValue <= totalChance)
        {
            GameManager.Instance.ActivateBetterEffect();
            GameManager.Instance.ChangeLives(2);
            Debug.Log("Gamble success! You get 1.5x life steal and double extra lives.");
        }
        else
        {
            GameManager.Instance.ChangeHealth(-GameManager.Instance.maxHealth);
            Debug.Log("Gamble failed! You die.");
        }

        CloseContractMenu();
}

    private void CloseContractMenu()
    {
        contractPanel.SetActive(false);
        devil1.SetActive(false);
    }

    public bool IsHealOnDamageActive()
    {
        return healOnDamageActive;
    }
}
