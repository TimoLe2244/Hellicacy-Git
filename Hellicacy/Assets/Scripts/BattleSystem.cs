using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private enum State {
        Idle,
        Active,
    }
    [SerializeField] private Wave[] waveArray;
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
            colliderTrigger.OnPlayerEnterBattle -= ColliderTrigger_OnPlayerEnterBattle;
        }
    }
    private void StartBattle()
    {
        Debug.Log("Starting Battle.");
        state = State.Active;
    }

    private void Update(){
        switch (state) {
            case State.Active:
                foreach (Wave wave in waveArray){
                wave.Update();
            }
            break;
        }
    }

    [System.Serializable]
    private class Wave {
        [SerializeField] private EnemySpawner[] enemySpawnerArray;
        [SerializeField] private float timer;

        public void Update(){
            if (timer >= 0) {
                timer -= Time.deltaTime;
                if (timer < 0) {
                    SpawnEnemies();
            }
        }
    }

        private void SpawnEnemies() {
            foreach (EnemySpawner enemySpawner in enemySpawnerArray){
                enemySpawner.Spawn();
            }
        }
    }
}
