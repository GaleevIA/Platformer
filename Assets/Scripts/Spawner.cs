using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameObject currentEnemy;
    public GameObject enemyPref;

    void Start()
    {
        currentEnemy = Instantiate(enemyPref, transform.position, Quaternion.identity);
    }

    
    void Update()
    {
        if(currentEnemy == null)
            currentEnemy = Instantiate(enemyPref, transform.position, Quaternion.identity);
    }
}
