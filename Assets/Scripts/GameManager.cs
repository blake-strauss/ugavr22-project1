using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    GameObject finishedKey;
    
    public enum GAME_STATE { IDLE, STARTED, FINISHED }
    public GAME_STATE gameState = GAME_STATE.IDLE;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        finishedKey = GameObject.Find("key");
        foreach (SearchObject so in objectsToFind)
        {
            so.gameObject.SetActive(false);
        }
        startButton.buttonPressed += startPressed;
        yield return new WaitForSeconds(0.0f);
        StartCoroutine(resetGame());
    }
    
    void startPressed(VRHand hand)
    {
        if (gameState == GAME_STATE.IDLE)
        {
            startGame();
        }
        else if (gameState == GAME_STATE.STARTED)
        {
            //reset game
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if objects in correct place, spawn the finished key
        bool spawnFinishedKey = true;
        foreach(TargetLocation t in targetLocations)
        {
            if (!t.isFound)
            {
                spawnFinishedKey = false;
                break;
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
            endGame();
        }
    }

    public void startGame()
    {
        player.doTeleport(mazeStartLocation.position);
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
        player.doTeleport(mazeStartLocation.position);
    }

    public IEnumerator resetGame()
    {
        yield return new WaitForSeconds(1.0f);
        player.doTeleport(startLocation.position);
        gameState = GAME_STATE.IDLE;
        yield return new WaitForSeconds(1.0f);

    }
}
