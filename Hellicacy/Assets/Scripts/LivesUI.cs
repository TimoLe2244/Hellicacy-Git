using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Image life1;
    public Image life2;
    public Image life3;

    public Color liveColor = Color.white;
    public Color deadColor = Color.black;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void UpdateLivesUI(int lives)
{
    if (life1 == null || life2 == null || life3 == null)
    {
        Debug.LogError("Lives images are not assigned in the Inspector!");
        return;
    }

    life1.color = (lives >= 1) ? liveColor : deadColor;
    life2.color = (lives >= 2) ? liveColor : deadColor;
    life3.color = (lives >= 3) ? liveColor : deadColor;
}


}
