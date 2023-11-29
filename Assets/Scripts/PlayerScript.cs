using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
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

    public void RangedAttack(Vector2 direction, Transform playerPosition, int distance)
    {
        //Debug.Log("player script runs");
        GameObject projectile = Instantiate(rangedProjectile, playerPosition.position, Quaternion.identity);
        Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();


        // Calculating angle for projectile to point towards enemy

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);


        rb2D.velocity = direction * projectileSpeed; 

        
        
    }



    /*
    public void MeleeAttack()
    {
        
    }
    */
}
