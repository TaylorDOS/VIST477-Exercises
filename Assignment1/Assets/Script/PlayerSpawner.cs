using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.CompilerServices;
using Photon.Pun.Demo.Cockpit;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject player;

    public Transform spawnPointsParent;
    private List<Transform> spawnPoints = new List<Transform>();

    private int count = 0;

    void Start()
    {
        foreach (Transform child in spawnPointsParent)
        {
            spawnPoints.Add(child);
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // Randomly choose a spawn point from the list
        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        Transform randomSpawnPoint = spawnPoints[spawnPointIndex];

       player = PhotonNetwork.Instantiate("Player", randomSpawnPoint.position, randomSpawnPoint.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(player);
    }
}
