using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
        
        
        
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPosition;
        bool positionOccupied;

        if (GameObject.FindGameObjectsWithTag("Enemy").Length < 34)
        {
            do
            {
                positionOccupied = false;
                float randomX = Mathf.Round(UnityEngine.Random.Range(-8.0f, 8.0f));
                float randomY = Mathf.Round(UnityEngine.Random.Range(3.0f, 4.0f));
                spawnPosition = new Vector3(randomX, randomY, 0);

                // Check current positions of all enemies
                GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in existingEnemies)
                {
                    if (Vector3.Distance(enemy.transform.position, spawnPosition) < 1)
                    {
                        positionOccupied = true;
                        break;
                    }
                }
            }
            while (positionOccupied);

            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            UnityEngine.Debug.Log("Enemy spawns are full");
        }
    }

}
