using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    private float lifetime = 1f; // Lifetime of the projectile in seconds

    private void Start()
    {
        // Schedule the projectile to be destroyed after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the projectile
            Destroy(gameObject);
        }
    }

}