using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //ConnectedtoServer();
        if (!PhotonNetwork.IsConnected)
        {
            ConnectedtoServer();
        }
        else
        {
            if (!PhotonNetwork.InRoom)
            {
                JoinOrCreateRoom();
            }
        }
    }


    // void Update()
    // {
    //     foreach (Player player in PhotonNetwork.PlayerList)
    // {
    //     if (!player.IsLocal)
    //     {
    //         player.tag = "Runner";
    //     }
    // }
    // }

    private void ConnectedtoServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Attempting connnection to server");
    }

    private void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Succesffuly connected");
        base.OnConnectedToMaster();

        if (!PhotonNetwork.InRoom)
        {
            JoinOrCreateRoom();
        }

        // RoomOptions roomOptions = new RoomOptions();
        // roomOptions.MaxPlayers = 4;
        // roomOptions.IsVisible  = true;
        // roomOptions.IsOpen = true;
        
        // PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined the Room!");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player joined the room!");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
