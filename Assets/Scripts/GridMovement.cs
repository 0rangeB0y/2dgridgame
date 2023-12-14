using System.Collections;
using System.Transactions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GridMovement : MonoBehaviour
{
    //Hold down key for movement
    //[SerializeField] private bool isRepeatedMovement = false;
    [SerializeField] private float moveDuration; //0.1f
    [SerializeField] private float gridSize; //1f
    [SerializeField] private int maxMovementOrig;
    

    
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private PlayerScript playerScript;

    private bool isMoving = false;
    //private bool canMove = true;

    int xClick;
    int yClick;
    int playerClickDistance;
    int enemyClickDistance;
    int xDistance;
    int yDistance;
    public int maxMovement;
    GameObject clickedEnemy;
    int enemiesToSpawn = 1;

    //int maxHealth = 10;
    public int currHealth = 10;
    public int turnNum = 1;
    public int roundNum = 1;

    float xCharacter;
    float yCharacter;

    private void Start()
    {
        maxMovement = maxMovementOrig;
        //SpawnEnemyFromGrid();
    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(nextTurn());
        }

        //Check to see if player killed all enemies
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < 1)
        {
            Debug.Log("Round complete");

            //Reset player back to start
            transform.position = new Vector3(0, -4, 0);
            roundNum++;
            turnNum = 0;
            enemiesToSpawn++;
            maxMovement = maxMovementOrig;
            if (roundNum < 11)
            {
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    SpawnEnemyFromGrid();
                }
            }
            else
            {
                SceneManager.LoadScene("Game Win");
            }

        }

        /*if(Input.GetKeyDown(KeyCode.E))
        {
            SpawnEnemyFromGrid();
        }*/
        


        if (Input.GetMouseButtonDown(0))
        {
            //Get Mouse coordinates
            Vector3 mousePos = Input.mousePosition;

            //Debug.Log("True X: " + mousePos.x);
            //Debug.Log("True Y: " + mousePos.y);


            xClick = (int)(math.round(mousePos.x / 108) - 9);
            yClick = (int)(math.round(mousePos.y / 108) - 5);

            //Debug.Log("Click X: " + xClick);
            //Debug.Log("Click Y: " + yClick);

            
            //Get Character coordinates
            Vector3 position = gameObject.transform.position;
            xCharacter = position.x;
            yCharacter = position.y;

            //Debug.Log("Player X: " + xCharacter);
            //Debug.Log("Player Y: " + yCharacter);

            //check if click on enemy
            bool clickEnemy = false;
            int xEnemy = -99;
            int yEnemy = -99;
            GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in existingEnemies)
            {
                if (enemy.transform.position.x == xClick && enemy.transform.position.y == yClick)
                {
                    clickEnemy = true;
                    xEnemy = (int)enemy.transform.position.x;
                    yEnemy = (int)enemy.transform.position.y;
                    clickedEnemy = enemy;

                    break; //break if enemy clicked is found
                }
            }

            if (clickEnemy)
            {
                Debug.Log("enemy clicked");

                if (xEnemy == -99 || yEnemy == -99)
                {
                    Debug.Log("Invalid enemy coordinates");
                }
                //Get distance from player to enemy
                xDistance = xEnemy - (int)xCharacter;
                yDistance = yEnemy - (int)yCharacter;
                enemyClickDistance = math.abs(xDistance) + math.abs(yDistance);

                //2 movement cost to attack
                if (maxMovement >= 2)
                { 
                    // Check range for attacks
                    // Check for melee attack first
                    if (enemyClickDistance <= 1 && maxMovement >= 2)
                    {
                        Debug.Log("melee attack");
                        DamageEnemy(2);
                        maxMovement -= 2;

                    } //Checks ranged attack if not in range for melee
                    else if (enemyClickDistance <= 3)
                    {
                        Debug.Log("ranged attack");
                        Vector2 direction = new Vector2(xDistance, yDistance);
                        PlayerRangedAttack(direction);
                        DamageEnemy(1);
                        maxMovement -= 2;
                    }
                }
                
                Debug.Log("Player Enemy distance: " + enemyClickDistance);

            }
            else
            {
                //Get Distance from click coordinates to player coordinates
                xDistance = xClick - (int)xCharacter;
                yDistance = yClick - (int)yCharacter;
                playerClickDistance = math.abs(xDistance) + math.abs(yDistance);
                Debug.Log("Player click distance: " + playerClickDistance);


                if (playerClickDistance <= maxMovement)
                {
                    //Debug.Log("Start movement");
                    StartCoroutine(HandleMovement());
                    maxMovement -= playerClickDistance;

                }
                //Debug.Log("max Movement: " + maxMovement);
            }
        }
        
    }



    public void SpawnEnemyFromGrid()
    {
        if (enemyScript != null)
        {
            enemyScript.SpawnEnemy();
        }
        else
        {
            Debug.LogError("Enemy script not assigned.");
        }
    }

    public void DamageEnemy(int damage)
    {
        if (enemyScript != null)
        {
            enemyScript.DamageEnemy(clickedEnemy, damage);
        }
        else
        {
            Debug.LogError("Enemy script not assigned.");
        }

    }

    public void DamagePlayer(int damage)
    {
        currHealth-= damage;

        if (currHealth <= 0)
        {
            Debug.Log("You lose");
            SceneManager.LoadScene("Game Loss");
        }





    }

    public void PlayerRangedAttack(Vector2 direction)
    {
        if (playerScript != null)
        {
        playerScript.RangedAttack(direction, transform, enemyClickDistance);
        }
        else
        {
            Debug.LogError("Player script not assigned.");
        }

    }

    private IEnumerator nextTurn()
    {
        //Advance turn counter
        turnNum++;

        //Get positioning
        Vector3 characterPosition = gameObject.transform.position;
        xCharacter = characterPosition.x;
        yCharacter = characterPosition.y;

        
        //Enemy code
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        yield return StartCoroutine(MoveEnemiesSequentially(enemies));


        //Reset Player movement
        maxMovement = maxMovementOrig;
        //Debug.Log("max Movement: " + maxMovement);
    }



    private IEnumerator HandleMovement()
    {
        // Handle xDistance
        while (xDistance != 0)
        {
            // Wait until the player is not moving
            yield return new WaitUntil(() => !isMoving);

            if (xDistance > 0)
            {
                StartCoroutine(Move(Vector2.right));
                xDistance -= 1;
            }
            else if (xDistance < 0)
            {
                StartCoroutine(Move(Vector2.left));
                xDistance += 1;
            }
            //Debug.Log("xdistance: " + xDistance);

        }

        // Handle yDistance
        while (yDistance != 0)
        {
            // Wait until the player is not moving
            yield return new WaitUntil(() => !isMoving);

            if (yDistance > 0)
            {
                StartCoroutine(Move(Vector2.up));
                yDistance -= 1;
            }
            else if (yDistance < 0)
            {
                StartCoroutine(Move(Vector2.down));
                yDistance += 1;
            }
        }
    }

    //Player movement
    private IEnumerator Move(Vector2 direction)
    {
        //Set that player is moving
        isMoving = true;
        //Get to and from coordinates
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);

        //Check to make sure movement is valid (within boundaries)
        if (math.abs(endPosition.x) <= 9 && math.abs(endPosition.y) <= 4)
        {
            float elapsedTime = 0;

            //Make movement smooth
            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float percent = elapsedTime / moveDuration;
                transform.position = Vector2.Lerp(startPosition, endPosition, percent);
                yield return null;
            }

            //Make sure correct position
            transform.position = endPosition;
        }

        //After movement is done set that player is no longer moving
        isMoving = false;
    }

    //Enemy movement

    private IEnumerator MoveEnemiesSequentially(GameObject[] enemies)
    {
        Debug.Log("move enemies sequentially");
        
        foreach (GameObject enemy in enemies)
        {
            //declare vars for unavailable positions
            Vector2 direction = Vector2.zero;
            bool upUnavailable = false;
            bool leftUnavailable = false;
            bool rightUnavailable = false;
            bool downUnavailable = false;
            bool moved = false;

            // Check current positions of all enemies
            GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject allEnemy in enemies)
            {
                if (((enemy.transform.position.x - allEnemy.transform.position.x) == 1) && (enemy.transform.position.y == allEnemy.transform.position.y))
                {
                    //Other enemy on left
                    leftUnavailable = true;
                    Debug.Log("left unavailable");
                }
                if (((enemy.transform.position.x - allEnemy.transform.position.x) == -1) && (enemy.transform.position.y == allEnemy.transform.position.y))
                {
                    //Other enemy on right
                    rightUnavailable = true;
                    Debug.Log("right unavailable");
                }
                if (((enemy.transform.position.y - allEnemy.transform.position.y) == 1) && (enemy.transform.position.x == allEnemy.transform.position.x))
                {
                    //Other enemy below
                    downUnavailable = true;
                    Debug.Log("below unavailable");
                }
                if (((enemy.transform.position.y - allEnemy.transform.position.y) == -1) && (enemy.transform.position.x == allEnemy.transform.position.x))
                {
                    //Other enemy above
                    upUnavailable = true;
                    Debug.Log("up unavailable");
                }
            }

            //Get distance from player to enemy
            xDistance = (int)enemy.transform.position.x - (int)xCharacter;
            yDistance = (int)enemy.transform.position.y - (int)yCharacter;
            int enemyPlayerDistance = math.abs(xDistance) + math.abs(yDistance);

            float enemyFromPlayerX = enemy.transform.position.x - xCharacter;
            float enemyFromPlayerY = enemy.transform.position.y - yCharacter;

            //Debug.Log("enemy from player X: " + enemyFromPlayerX);
            //Debug.Log("enemy from player Y: " + enemyFromPlayerY);
            

            if (enemyPlayerDistance > 1)
            {
                if (math.abs(enemyFromPlayerX) >= math.abs(enemyFromPlayerY))
                {
                    if (enemyFromPlayerX > 0 &! leftUnavailable)
                    {
                        //Go Left
                        direction = Vector2.left;
                        moved = true;
                    }
                    else if (enemyFromPlayerX < 0 &! rightUnavailable)
                    {
                        //Go Right
                        direction = Vector2.right;
                        moved = true;
                    }
                }
                if (math.abs(enemyFromPlayerX) < math.abs(enemyFromPlayerY) &! moved)
                {
                    if (enemyFromPlayerY > 0 &! downUnavailable)
                    {
                        //Go Down
                        direction = Vector2.down;
                        moved = true;
                    }
                    else if (enemyFromPlayerY < 0 &! upUnavailable)
                    {
                        //Go Up
                        direction = Vector2.up;
                        moved = true;
                    }
                }
                //yield return StartCoroutine(MoveEnemy(enemy, direction));
                if (!moved)
                {
                    Debug.Log("enemy cant move normally");
                    if(upUnavailable || downUnavailable)
                    {
                        if(!rightUnavailable)
                        {
                            direction = Vector2.right;
                        }
                        else if(!leftUnavailable)
                        {
                            direction = Vector2.left;
                        }
                        else
                        {
                            Debug.Log("Enemy cant move at all");
                        }
                    }
                    else if(rightUnavailable || leftUnavailable)
                    {
                        if (!downUnavailable)
                        {
                            direction = Vector2.down;
                        }
                        else if (!upUnavailable)
                        {
                            direction = Vector2.up;
                        }
                        else
                        {
                            Debug.Log("Enemy cant move at all");
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Player attacked");
                DamagePlayer(1);
                //Maybe do projectile?
            }

            // Start moving the enemy and wait until the move is complete
            yield return StartCoroutine(MoveEnemy(enemy, direction));
        }
    }

    private IEnumerator MoveEnemy(GameObject enemy, Vector2 direction)
    {
        Vector2 startPosition = enemy.transform.position;
        Vector2 endPosition = startPosition + (direction * gridSize);
        float enemyMoveDuration = 0.5f;

        float elapsedTime = 0;

        while (elapsedTime < enemyMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / enemyMoveDuration;
            enemy.transform.position = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }

        enemy.transform.position = endPosition;
    }



    // Enemy must be next to the player in order for the attack to go through.
    private bool IsEnemyAdjacent(GameObject enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        return distance <= 1.0;
    }

}



