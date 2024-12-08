using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] bool closedByDefault = true;

    void Awake()
    {
        if(closedByDefault)
        {
            CloseMenu();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetComponent<Canvas>().enabled)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu(){
        Time.timeScale = 0;
        GetComponent<Canvas>().enabled = true;
    }

    public void CloseMenu(){
        Time.timeScale = 1;
        GetComponent<Canvas>().enabled = false;
    }

    public void MainMenu(){
        SceneManager.LoadScene("main_menu");
    }
}
