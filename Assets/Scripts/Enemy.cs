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

    //List for current spawn Positions of enemies
    List<Vector3> spawnedPositions = new List<Vector3>();

    public void SpawnEnemy()
    {
        Vector3 spawnPosition;
        bool positionOccupied;

        if (spawnedPositions.Count < 38)
        {
            do
            {
                positionOccupied = false;
                float randomX = Mathf.Round(UnityEngine.Random.Range(-9.0f, 9.0f));
                float randomY = Mathf.Round(UnityEngine.Random.Range(3.0f, 4.0f));
                spawnPosition = new Vector3(randomX, randomY, 0);

                // Check if any enemy has already spawned at this position
                foreach (Vector3 pos in spawnedPositions)
                {
                    if (Vector3.Distance(pos, spawnPosition) < 1) // Check to see if enemy spawns on occupied tile
                    {
                        positionOccupied = true;
                        break;
                    }
                }
            }
            while (positionOccupied);


            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedPositions.Add(spawnPosition); // Add the spawn position to the list
        }
        else
        {
            UnityEngine.Debug.Log("Enemy spawns are full");
        }
    }

}
