using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class CharacterCreationMenu : MonoBehaviour
{
    public GameObject character;
    public List<OutfitChanger> outfitChangers = new List<OutfitChanger>();

    public void RandomizeCharacter()
    {
        foreach (OutfitChanger changer in outfitChangers)
        {
            changer.Randomize();

        }
    }

    public void Submit()
    {
        SceneManager.LoadScene("restaurant");
    }

    
}
