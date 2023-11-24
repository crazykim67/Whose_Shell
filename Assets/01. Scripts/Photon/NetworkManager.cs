using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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

    private const string ROOM_CODE = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

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

    #region Connect

    public void OnConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region Disconnect

    public void OnDisconnect()
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    #endregion

    #region Room

    // 방 만들기
    public void CreateRoom(int maxPlayerCount, int terrapinCount)
    {
        if (!PhotonNetwork.IsConnected)
        {
            OnConnect();
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayerCount;
        roomOptions.CustomRoomProperties = new Hashtable() { {"Terrapin", terrapinCount } };
        PhotonNetwork.CreateRoom(OnCreateRoomCode(), roomOptions);
    }

    // 방 입장
    public void JoinRoom(string _roomName)
    {
        PhotonNetwork.JoinRoom(_roomName);
    }

    // 방 나가기
    public void LeftRoom()
    {
        if (!PhotonNetwork.InRoom)
            return;

        if(PhotonNetwork.CurrentRoom.PlayerCount <= 1)
            FirebaseManager.Instance.RemoveRoomData(PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region 포톤 콜백

    // 연결 콜백
    public override void OnConnectedToMaster()
    {
        Debug.Log("Server Connect To Successful...!");
    }

    // 연결 끊기 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Server Disconnected...!");
    }

    // 방 만들기 콜백
    public override void OnCreatedRoom()
    {
        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        int terrapin = (int)cp["Terrapin"]; 
        FirebaseManager.Instance.OnRoomData(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount);
        Debug.Log("Create Room Successful...!");
    }

    // 방 입장 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room Successful...!");
        FirebaseManager.Instance.UpdatePlayerCount(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount);

        PhotonNetwork.InstantiateRoomObject("GameManager", Vector3.zero, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject("CustomManager", Vector3.zero, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject("PlayerListCanvas", Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        // Vivox 연결
        VivoxManager.Instance.Login(AuthManager.Instance.currentId);

        VivoxManager.Instance.OnInputAudioMute(true);
        VivoxManager.Instance.OnOutputAudioMute(true);
    }

    // 방 나가기 콜백
    public override void OnLeftRoom()
    {
        Debug.Log("On Left Room...!");
        NetworkManager.Instance.OnLoadScene("MainMenu");
        VivoxManager.Instance.OnLeft();
    }

    #endregion

    public string OnCreateRoomCode()
    {
        var sb = new System.Text.StringBuilder(5);
        var r = new System.Random();

        for(int i = 0; i < 5; i++)
        {
            int pos = r.Next(ROOM_CODE.Length);
            char c = ROOM_CODE[pos];
            sb.Append(c);
        }
        // 중복검사해야함
        return sb.ToString();
    }

    public void OnApplicationQuit()
    {
        if (PhotonNetwork.InRoom)
            LeftRoom();

        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public void OnLoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
