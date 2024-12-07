using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private enum State {
        Idle,
        Active,
    }
    [SerializeField] private Transform enemySpawner;
    [SerializeField] private ColliderTrigger colliderTrigger;
    private State state;
    private void Awake()
    {
        state = State.Idle;
    }

    private void Start()
    {
        colliderTrigger.OnPlayerEnterBattle += ColliderTrigger_OnPlayerEnterBattle;
    }

    private void ColliderTrigger_OnPlayerEnterBattle(object sender, System.EventArgs e)
    {
        if (state == State.Idle)
        {
            StartBattle();
        }
    }
    private void StartBattle()
    {
        Debug.Log("Starting Battle.");
        enemySpawner.GetComponent<EnemySpawner>().Spawn();
        state = State.Active;
    }
}
