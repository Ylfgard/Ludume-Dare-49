using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Patrol> patrols;
    public List<EnemyLogic> enemies;
    private void Start()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(true);
            enemies[i].PatrolPoints = patrols[i].wayPoints;
        }
    }
}


