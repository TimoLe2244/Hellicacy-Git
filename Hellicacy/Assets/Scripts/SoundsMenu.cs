using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsMenu : MonoBehaviour
{
    [SerializeField] bool closedByDefault = true;

    void Awake()
    {
        if(closedByDefault)
        {
            CloseMenu();
        }
    }
    public void OpenMenu(){
        GetComponent<Canvas>().enabled = true;
    }

    public void CloseMenu(){
        GetComponent<Canvas>().enabled = false;
    }
}
