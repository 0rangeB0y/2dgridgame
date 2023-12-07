using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualEnemy : MonoBehaviour
{

    public int health = 5;


    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject); // destroys the Enemy(Clone) GameObject
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
