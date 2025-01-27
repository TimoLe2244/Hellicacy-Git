using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public event EventHandler OnPlayerEnterBattle;
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player"){
            OnPlayerEnterBattle?.Invoke(this, EventArgs.Empty);
        }
    }
}
