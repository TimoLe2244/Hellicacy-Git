using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitChanger : MonoBehaviour
{
    public SpriteRenderer bodyPart;

    public List<Sprite> options = new List<Sprite>();

    public int currentOption = 0;
    
    public void NextOption()
    {
        currentOption++;
        if(currentOption >= options.Count)
        {
            currentOption = 0;
        }

        bodyPart.sprite = options[currentOption];
    }
    public void PreviousOption()
    {
        currentOption--;
        if(currentOption < 0)
        {
            currentOption = options.Count - 1;
        }
        
        bodyPart.sprite = options[currentOption];
    }

    public void Randomize()
    {
        currentOption = Random.Range(0, options.Count - 1);
        bodyPart.sprite = options[currentOption];
    }
}
