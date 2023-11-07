using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    [SerializeField] public GameObject enemyPrefab; // Assign your enemy prefab here
    private GameObject enemyInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (enemyInstance == null)
        {
            Vector3 spawnPosition = new Vector3(0, 3, 0);


            enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
