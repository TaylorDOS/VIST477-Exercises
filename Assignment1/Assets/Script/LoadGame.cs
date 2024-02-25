using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoadGame : MonoBehaviourPunCallbacks
{
    public int minPlayersRequired = 2;

    void Start()
    {
        CheckPlayerCount();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        CheckPlayerCount();
    }

    private void CheckPlayerCount()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount >= minPlayersRequired)
        {
            PhotonNetwork.LoadLevel("UserTracking");
        }
    }
}
