using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagAssigner : MonoBehaviour
{
    private string currentTag;
    public Canvas DesktopUI, HMDUi;
    public Text desktopRoleText, HMDRoleText, desktoptimer,HMDTimer;
    public RawImage[] desktophealth = new RawImage[3], HMDImagehealth= new RawImage[3];
    private float startTime=120.0f;
    private int health=3;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTag = gameObject.tag;
        // Find the Text GameObject with the specified name within the DesktopUI Canvas
        /*
        Transform desktopTextTransform = DesktopUI.transform.Find("Role");
        Transform desktopHealthTransform = DesktopUI.transform.Find("Health");
        if (desktopTextTransform != null)
        {
            desktopRoleText = desktopTextTransform.GetComponent<Text>();
        }
        if (desktopHealthTransform != null)
        {
            desktopHealthText = desktopHealthTransform.GetComponent<Text>();
            desktopHealthText.text = health.ToString();
        }

        // Find the Text GameObject with the specified name within the HMDUi Canvas
        Transform hmdTextTransform = HMDUi.transform.Find("Role");
        Transform hmdHealthTransform = HMDUi.transform.Find("Health");
        if (hmdTextTransform != null)
        {
            HMDRoleText = hmdTextTransform.GetComponent<Text>();
        }
        if (hmdHealthTransform != null)
        {
            HMDHealthText = hmdHealthTransform.GetComponent<Text>();
            HMDHealthText.text = health.ToString();
        }
        */
    }

    // Update is called once per frame
    void LateUpdate()
    {
       // HMDRoleText.text = currentTag;
       if(desktopRoleText!= null)
            desktopRoleText.text = currentTag;
    if(desktophealth!=null && HMDImagehealth!=null){
        if (health == 3 )
        {
            desktophealth[0].enabled = true;
            desktophealth[1].enabled = true;
            desktophealth[2].enabled = true;
            HMDImagehealth[0].enabled = true;
            HMDImagehealth[1].enabled = true;
            HMDImagehealth[2].enabled = true;
        }
        else if (health == 2)
        {
            desktophealth[0].enabled = true;
            desktophealth[1].enabled = true;
            desktophealth[2].enabled = false;
            HMDImagehealth[0].enabled = true;
            HMDImagehealth[1].enabled = true;
            HMDImagehealth[2].enabled = false;
        }
        else if (health == 1)
        {
            desktophealth[0].enabled = true;
            desktophealth[1].enabled = false;
            desktophealth[2].enabled = false;
            HMDImagehealth[0].enabled = true;
            HMDImagehealth[1].enabled = false;
            HMDImagehealth[2].enabled = false;
        }
        else if (health == 0)
        {
            desktophealth[0].enabled = false;
            desktophealth[1].enabled = false;
            desktophealth[2].enabled = false;
            HMDImagehealth[0].enabled = false;
            HMDImagehealth[1].enabled = false;
            HMDImagehealth[2].enabled = false;
        }
    }
        // Check if the current tag is different from the initial tag
        if (gameObject.tag != currentTag)
        {
            Debug.Log("<Tag Assigner>Tag has been changed from " + currentTag + " to " + gameObject.tag);
            if (currentTag == "Runner" && gameObject.tag == "Tagger")
            {
                health-=1;
              //  HMDHealthText.text = health.ToString();
              //  desktopHealthText.text = health.ToString();
            }
            currentTag = gameObject.tag; // Update the currentTag variable
            
        }

    if(currentTag=="Tagger")
    {
        startTime -= Time.deltaTime;
        
        
        if (startTime <= 0)
        {
            health -= 1;
            startTime = 120.0f;
            
        }
        desktoptimer.text = startTime.ToString();
            HMDTimer.text = startTime.ToString();


    }


    if(health==0)
    {
        //End the game
         
                if (isPaused)
                {
                    Time.timeScale = 0; // Pause the game
                }
                else
                {
                    Time.timeScale = 1; // Resume the game
                }
    }


    }
}


/*
public override void OnJoinedRoom()
{
    base.OnJoinedRoom();
    // Randomly choose a spawn point from the list
    int spawnPointIndex = Random.Range(0, spawnPoints.Count);
    Transform randomSpawnPoint = spawnPoints[spawnPointIndex];

    player = PhotonNetwork.Instantiate("Player", randomSpawnPoint.position, randomSpawnPoint.rotation);

    // Display the player count
    Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
}

public override void OnPlayerEnteredRoom(Player newPlayer)
{
    base.OnPlayerEnteredRoom(newPlayer);
    // Display the player count when a new player joins the room
    Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
}

public override void OnPlayerLeftRoom(Player otherPlayer)
{
    base.OnPlayerLeftRoom(otherPlayer);
    // Display the player count when a player leaves the room
    Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
}
*/