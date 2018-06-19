using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class MatchAssetsLoader : MonoBehaviour
{
    private void Awake()
    {
        SetupFakeClient();
        SetupFakeServer();

        // Rest of initialization

        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.MasterClient
        };

        PhotonNetwork.RaiseEvent(EventCodes.N_PLAYER_READY, null, true, options);
    }

    private void SetupFakeClient()
    {
        GameObject fakeClient = new GameObject("FakeClient");
        fakeClient.AddComponent<FakeClient>();
    }

    private void SetupFakeServer()
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject fakeServer = new GameObject("FakeServer");
            fakeServer.AddComponent<FakeServer>();
        }
    }
}
