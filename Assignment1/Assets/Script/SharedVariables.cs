using UnityEngine;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic; 
using Photon.Realtime;
using Photon.Pun;


public class SharedVariables : MonoBehaviourPunCallbacks, IPunObservable, IPunInstantiateMagicCallback
{
    
    private List<string> count = new List<string>();
    private int num =0,temp =0;
    [PunRPC]
    public void AddToCount()
    {
        //count += valueToAdd;
        if(num == 0){
            count.Add("Tagger");
        }
        else
            {count.Add("Runner");
            }
        num++;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(num);
        }
        else
        {
            num = (int)stream.ReceiveNext();
        }
    }


    public void OnPhotonInstantiate(Photon.Pun.PhotonMessageInfo info)
    {
        photonView.RPC("AddToCount", RpcTarget.All);
        
        StartCoroutine(DelayedRoleAssignment());
    }

    private IEnumerator DelayedRoleAssignment()
    {
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("Count = " + count);
       
            
                
        temp =0;
        
          //  gameObject.tag = count[temp];
        Debug.Log("Player size " + PhotonNetwork.PlayerList.Length);
        Debug.Log("Player num " + num);

        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        if (photonView != null && photonView.Owner != null)
        {
            int actorNumber = photonView.Owner.ActorNumber;
            Debug.Log("Actor Number: " + photonView.Owner.ActorNumber);
            temp=actorNumber-1;
        }




    //     foreach (Player player in PhotonNetwork.PlayerList)
    // {
    //      Debug.Log("Player " + player.IsLocal + " is " + player.ActorNumber + " is " );
    //     if (player.IsLocal)
    //     {
    //         //player.TagObject = "Tagger";//count[temp];
    //         temp=player.ActorNumber;
            
    //         // gameObject.tag = count[temp-1];
    //     }
        
    //   //  Debug.Log("Player " + player.IsLocal + " is " + player.ActorNumber);
    //    // Debug.Log("Player " + temp + " is " + count[temp]);
        
    // }
    
           
           
    }

    public string returnTag()
    {
        return count[temp];
    }

}
