using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TaggingMechanism : MonoBehaviourPunCallbacks
{
    public Material blueRunner;
    public Material redTagger;
    private Renderer objectRenderer;

    private string currentTag;
    public GameObject HMDUi;
    private Text HMDRoleText;
    private Text HMDPowerUpText;
    private Text HMDTimerText;
    private GameObject[] HMDHealthIcon;
    private float totalTime = 10f;
    private float  timeLeft;
    private int heart = 3;
    private int nextToDeactivateIndex = 0;

    private SharedVariables sharedVariables;
    private bool timerStarted = false;

    void Start()
    {
        // if (photonView.IsMine)
        // {
        //     if (PhotonNetwork.IsMasterClient)
        //     {
        //         SetPlayerRole("Tagger");
        //     }
        //     else
        //     {
        //         SetPlayerRole("Runner");
        //     }
        // }
        sharedVariables = gameObject.GetComponent<SharedVariables>();
        //currentTag = gameObject.tag;
        objectRenderer = GetComponent<Renderer>();

        Transform hmdTextTransform = HMDUi.transform.Find("Role");
        Transform hmdPowerUpTransform = HMDUi.transform.Find("PowerUp");
        Transform hmdTimerTransform = HMDUi.transform.Find("Timer");
        Transform hmdHealthPanelTransform = HMDUi.transform.Find("HealthPanel");

        // Assign Text components if the corresponding UI elements are found
        if (hmdTextTransform != null)
        {
            HMDRoleText = hmdTextTransform.GetComponent<Text>();
        }
        if (hmdPowerUpTransform != null)
        {
            HMDPowerUpText = hmdPowerUpTransform.GetComponent<Text>();
        }
        if (hmdTimerTransform != null)
        {
            HMDTimerText = hmdTimerTransform.GetComponent<Text>();
        }
        if (hmdHealthPanelTransform != null)
        {
            int childCount = hmdHealthPanelTransform.childCount;
            HMDHealthIcon = new GameObject[childCount];
            
            for (int i = 0; i < childCount; i++)
            {
                HMDHealthIcon[i] = hmdHealthPanelTransform.GetChild(i).gameObject;
            }
        }
        StartCoroutine(InitiliazePlayer());
        
    }

    [PunRPC]
    void SetPlayerRole(string role)
    {
        if (role == "Tagger")
        {
            gameObject.tag = "Tagger";
            objectRenderer.material = redTagger;
            HMDRoleText.text = "Tagger";
        }
        else if (role == "Runner")
        {
            gameObject.tag = "Runner";
            objectRenderer.material = blueRunner;
            HMDRoleText.text = "Runner";
        }
    }

    void Update()
    {
        string tag = sharedVariables.returnTag();
        //if (count >= 2 && !timerStarted)
        //{
           // StartCoroutine(Timer());
            //timerStarted = true;
            //Debug.Log("player > 2");
        //}
        gameObject.tag = tag;
    }

    IEnumerator InitiliazePlayer()
    {
        yield return new WaitForSeconds(1);
        if (gameObject.CompareTag("Runner"))
        {
            objectRenderer.material = blueRunner;
            HMDRoleText.text = "Runner";
        }
        else if (gameObject.CompareTag("Tagger"))
        {
            objectRenderer.material = redTagger;
            HMDRoleText.text = "Tagger";
        }
    }

    IEnumerator Timer()
    {
        timeLeft = totalTime;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
            HMDTimerText.text = Mathf.FloorToInt(timeLeft).ToString() + "sec";
        }
        if (gameObject.CompareTag("Tagger"))
            {
                MinusHeart();
            }
        // If time runs out
        //HMDTimerText.text = "Time's up!";
        //NextLevel();
    }

    public void UpdatePowerUpUI(string type)
    {
        HMDPowerUpText.text = type;
    }

    void OnCollisionEnter(Collision collision)
    {
        // if (gameObject.CompareTag("Tagger") && collision.gameObject.CompareTag("Runner"))
        // {
        //     StartCoroutine(changeToRunner(collision.gameObject, 2));
        // }
        if (gameObject.CompareTag("Runner") && collision.gameObject.CompareTag("Tagger"))
        {
            StartCoroutine(changeToTagger(collision.gameObject, 2));
        }
    }

    // IEnumerator changeToRunner(GameObject otherObject, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     currentTag = "Runner";
    //     gameObject.tag = "Runner";
    //     otherObject.tag = "Tagger";
    //     objectRenderer.material = blueRunner;
    //     otherObject.GetComponent<Renderer>().material = redTagger;
    //     HMDRoleText.text = "Runner";
    // }

    IEnumerator changeToTagger(GameObject otherObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        currentTag = "Tagger";
        gameObject.tag = "Tagger";
        otherObject.tag = "Runner";
        objectRenderer.material = redTagger;
        otherObject.GetComponent<Renderer>().material = blueRunner;
        HMDRoleText.text = "Tagger";
        otherObject.GetComponent<TaggingMechanism>().HMDRoleText.text = "Runner";
        MinusHeart();
    }
    void MinusHeart()
    {
        heart -=1;
        Debug.Log("heart: " + heart);
        if (HMDHealthIcon != null && HMDHealthIcon.Length > 0 && nextToDeactivateIndex < HMDHealthIcon.Length)
        {
            HMDHealthIcon[nextToDeactivateIndex].SetActive(false);
            nextToDeactivateIndex++;
        }
        if (heart<1)
        {
            EndGame();
        }
        else
        {
           // StartCoroutine(Timer());
        }
    }
    // void NextLevel()
    // {
    //     level += 1;
    //     if (level > 3)
    //     {
    //         if (gameObject.CompareTag("Tagger"))
    //         {
    //             WinGame();
    //         }
    //         else if (gameObject.CompareTag("Runner"))
    //         {
    //             EndGame();
    //         }
    //     }
    //     else
    //     {
    //         // if (gameObject.CompareTag("Tagger"))
    //         // {
    //         //     gameObject.tag = "Runner";
    //         //     objectRenderer.material = blueRunner;
    //         //     HMDRoleText.text = "Runner";
    //         //     MinusHeart();
    //         // }
    //         // else if (gameObject.CompareTag("Runner"))
    //         // {
    //         //     gameObject.tag = "Tagger";
    //         //     objectRenderer.material = redTagger;
    //         //     HMDRoleText.text = "Tagger";
    //         // }
    //         StartCoroutine(Timer());
    //     }
    // }
    void WinGame()
    {
        HMDRoleText.text = "Game Over, you win";
        Time.timeScale = 0;
    }
    void EndGame()
    {
        HMDRoleText.text = "Game Over, you lost";
        SC_RigidbodyWalker rigidbodyWalker = gameObject.GetComponent<SC_RigidbodyWalker>();
        if (rigidbodyWalker != null)
        {
            rigidbodyWalker.enabled = false;
        }
    }
}
