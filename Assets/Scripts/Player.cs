using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject rangedProjectile;
    [SerializeField] private float projectileSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RangedAttack(Vector2 direction)
    {
        GameObject projectile = Instantiate(rangedProjectile, transform.position, Quaternion.identity);
        Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
        rb2D.velocity = direction * projectileSpeed; 
    }

    /*
    public void MeleeAttack()
    {
        
    }
    */
}
