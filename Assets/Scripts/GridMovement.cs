using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class NewBehaviourScript : MonoBehaviour
{
    //Hold down key for movement
    [SerializeField] private bool isRepeatedMovement = false;
    [SerializeField] private float moveDuration; //0.1f
    [SerializeField] private float gridSize; //1f
    [SerializeField] private int maxMovementOrig;

    private bool isMoving = false;
    //private bool canMove = true;

    int xClick;
    int yClick;
    int playerClickDistance;
    int xDistance;
    int yDistance;
    int maxMovement;
    //int currDistance;

    private void Start()
    {
        maxMovement = maxMovementOrig;
    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            nextTurn();
        }


        if (Input.GetMouseButtonDown(0))
        {
            //Get Mouse coordinates
            Vector3 mousePos = Input.mousePosition;

            //Debug.Log("True X: " + mousePos.x);
            //Debug.Log("True Y: " + mousePos.y);

            xClick = (int)(math.round(mousePos.x / 72.5) - 10);
            yClick = (int)(math.round(mousePos.y / 72.5) - 5);

            Debug.Log("Click X: " + xClick);
            Debug.Log("Click Y: " + yClick);

            //Get Character coordinates
            Vector3 position = gameObject.transform.position;
            float xCharacter = position.x;
            float yCharacter = position.y;

            Debug.Log("Player X: " + xCharacter);
            Debug.Log("Player Y: " + yCharacter);

            //Get Distance from click coordinates to player coordinates
            xDistance = xClick - (int)xCharacter;
            yDistance = yClick - (int)yCharacter;
            playerClickDistance = math.abs(xDistance) + math.abs(yDistance);
            Debug.Log("distance: " + playerClickDistance);
            

            if (playerClickDistance <= maxMovement)
            {
                Debug.Log("Start movement");
                StartCoroutine(HandleMovement());
                maxMovement -= playerClickDistance;
               
            }
            Debug.Log("max Movement: " + maxMovement);
        }
        


        if (!isMoving)
        {
            //Makes it so you cant spam movement with arrow keys
            System.Func<KeyCode, bool> inputFunction;
            if (isRepeatedMovement)
            {
                inputFunction = Input.GetKey;
            }
            else
            {
                inputFunction = Input.GetKeyDown;
            }

            //if function active, move in direction
            if (inputFunction(KeyCode.UpArrow))
            {
                StartCoroutine(Move(Vector2.up));
            }
            else if (inputFunction(KeyCode.DownArrow))
            {
                StartCoroutine(Move(Vector2.down));
            }
            else if (inputFunction(KeyCode.LeftArrow))
            {
                StartCoroutine(Move(Vector2.left));
            }
            else if (inputFunction(KeyCode.RightArrow))
            {
                StartCoroutine(Move(Vector2.right));
            }
        }
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


    private void nextTurn()
    {
        //Enemy code




        //Reset Player
        maxMovement = maxMovementOrig;
        Debug.Log("max Movement: " + maxMovement);
    }


    private void spawnEnemy()
    {


    }
    public class SpawnEnemy : MonoBehaviour
    {
        public GameObject enemy; // Assign your enemy game object here

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E)) // When the E key is pressed
            {
                enemy.SetActive(true); // Activate the enemy
            }
        }
    }


}



