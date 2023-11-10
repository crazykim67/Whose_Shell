using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Instance

    private static NetworkManager instance;

    public static NetworkManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new NetworkManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        OnConnect();
    }

    public void OnConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Server Connect To Successful...!");
    }

    public void OnDisconnect()
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Server Disconnected...!");
    }
}
