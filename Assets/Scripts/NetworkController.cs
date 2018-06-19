using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : Photon.PunBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings("space_battles_prototype");
    }

    GUIStyle style = new GUIStyle();

    void OnGUI()
    {
        style.normal.textColor = Color.white;
        style.padding = new RectOffset(30, 0, 30, 0);
        style.fontSize = 40;
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString(), style);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        RoomOptions ro = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("test_room", ro, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (PhotonNetwork.isMasterClient
            && SceneManager.GetActiveScene().name == "Pregame"
            && PhotonNetwork.room.PlayerCount == 2)
        {
            LoadGame();
        }
    }

    void LoadGame()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
