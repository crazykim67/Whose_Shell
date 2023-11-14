using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameManager();
                return instance;
            }

            return instance;
        }
    }

    [HideInInspector]
    public int terrapinCount;

    private void Awake()
    {
        instance = this;
    }

    public override void OnJoinedRoom()
    {
        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        int terrapin = (int)cp["Terrapin"];

        terrapinCount = terrapin;


        FirebaseManager.Instance.OnLoadData();
    }
}
