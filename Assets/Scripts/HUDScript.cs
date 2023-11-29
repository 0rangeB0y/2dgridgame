using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDScript : MonoBehaviour
{

    public TextMeshProUGUI hudText;
    private GridMovement gameData; // Reference to your game data


    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GridMovement>(); // Or any other way to get your game data
    }

    // Update is called once per frame
    void Update()
    {
        hudText.text = "Health: " + gameData.currHealth + "\nMovement available: " + gameData.maxMovement + "\nTurn : " + gameData.turnNum; 
    }
}



