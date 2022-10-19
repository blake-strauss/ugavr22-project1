using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //directly assigned in the inspector
    [SerializeField] VRPlayer player;
    [SerializeField] List<SearchObject> objectsToFind; 
    [SerializeField] List<HidingLocation> hidingLocations; 
    [SerializeField] List<TargetLocation> targetLocations;
    [SerializeField] Transform startLocation;
    [SerializeField] Transform mazeStartLocation;
    [SerializeField] StartButton startButton;
    [SerializeField] Door exitDoor;
    [SerializeField] TMP_Text instructionText;
    GameObject finishedKey;
    
    public enum GAME_STATE { IDLE, STARTED, FINISHED }
    public GAME_STATE gameState = GAME_STATE.IDLE;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        player.doTeleport(startLocation.position, startLocation.rotation);
        finishedKey = GameObject.Find("key");
        foreach (SearchObject so in objectsToFind)
        {
            so.gameObject.SetActive(false);
        }
        startButton.buttonPressed += startPressed;
        yield return new WaitForSeconds(0.0f);;
    }
    
    void startPressed(VRHand hand)
    {
        if (gameState == GAME_STATE.IDLE)
        {
            startGame();
        }
        else if (gameState == GAME_STATE.STARTED)
        {
            
        }
        else if (gameState == GAME_STATE.FINISHED)
        {
            resetGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if objects in correct place, spawn the finished key
        bool spawnFinishedKey = true;
        foreach(TargetLocation t in targetLocations)
        {
            if (t.clearHand)
            {
                t.clearHand = false;
                player.hands[0].grabbables.Clear();
                player.hands[1].grabbables.Clear();
            }
            if (!t.isFound)
            {
                spawnFinishedKey = false;
                //break;
            }
        }
        if (spawnFinishedKey)
        {
            finishedKey.SetActive(true);
            finishedKey.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        
        //if the key is inserted into the door, player wins game
        if (exitDoor.isUnlocked)
        {
            exitDoor.isUnlocked = false;
            endGame();
        }
    }

    public void startGame()
    {
        player.doTeleport(mazeStartLocation.position, mazeStartLocation.rotation);
        foreach (SearchObject so in objectsToFind)
        {
            so.gameObject.SetActive(true);
        }
        
        List<HidingLocation> hidingTemp = new List<HidingLocation>(hidingLocations);
        foreach(SearchObject so in objectsToFind)
        {
            //choose a hiding location randomly that isn't already chosen
            int loc = Random.Range(0, hidingTemp.Count);
            so.transform.position = hidingTemp[loc].transform.position;
            hidingTemp.RemoveAt(loc);
        }
        finishedKey.SetActive(false);
        gameState = GAME_STATE.STARTED;
    }

    public void endGame()
    {
        player.doTeleport(startLocation.position, startLocation.rotation);
        gameState = GAME_STATE.FINISHED;
        instructionText.text = "Congratulations! You escaped the maze!\nPress the green button to play again!\n\n" +
                               "Credits: Dr. Kyle Johnsen - item manipulation, locomotion\nnewagesoup - wink sound effect\nAndrey Ferar - door texture\nLowlyPoly - grass texture\n" +
                               "Game-Ready Studios - wall texture";
    }

    public void resetGame()
    {
        gameState = GAME_STATE.IDLE;
        SceneManager.LoadScene(0);
    }
}
