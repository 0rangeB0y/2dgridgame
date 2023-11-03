using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject enemyPrefab; // Assign your enemy prefab here
    private GameObject enemyInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        if (enemyInstance == null)
        {
            enemyInstance = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
