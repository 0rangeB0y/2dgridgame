using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IndividualEnemy : MonoBehaviour
{

    public int health = 5;

    //public GameObject healthBarPrefab;
    //private GameObject healthBarUI;
    [SerializeField] FloatingHealthBar healthBar;



    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.updateHealthBar(health, 5);
        Debug.Log("Enemy health: " + health);
        if (health <= 0)
        {
            //Destroy(healthBarUI);
            Destroy(gameObject); // destroys the Enemy(Clone) GameObject
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        healthBar.updateHealthBar(health, 5);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


}
