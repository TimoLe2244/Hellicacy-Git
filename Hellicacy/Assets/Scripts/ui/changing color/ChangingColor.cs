using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingColor : MonoBehaviour
{
    public GameObject panel;
    public SpriteRenderer hair;

    public Color black;
    public Color red;
    public Color blue;
    public Color white;

    public int whatColor = 1;

    void Update(){
        if(whatColor == 1){
            hair.color = black;
            
        } else if (whatColor == 2){
            hair.color = red;
        } else if (whatColor == 3){
            hair.color = blue;
        } else if (whatColor == 4){
            hair.color = white;
        }
    }
    public void OpenPanel(){
        panel.SetActive(true);
    }

    public void ClosePanel(){
        panel.SetActive(false);
    }

    public void ChangeHairBlack(){
        whatColor = 1;
    }
    public void ChangeHairRed(){
        whatColor = 2;
    }
    public void ChangeHairBlue(){
        whatColor = 3;
    }
    public void ChangeHairWhite(){
        whatColor = 4;
    }
}
