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
    public void CreateRoom(int maxPlayerCount)
    {
        if (!PhotonNetwork.IsConnected)
        {
            OnConnect();
            return;
        }

        PhotonNetwork.CreateRoom(OnCreateRoomCode(), new RoomOptions { MaxPlayers = maxPlayerCount });
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
        {
            FirebaseManager.Instance.RemoveRoomData(PhotonNetwork.CurrentRoom.Name);
            Debug.Log("방 파괴");
        }

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
        Debug.Log("Create Room Successful...!");
        FirebaseManager.Instance.OnRoomData(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount);
    }

    // 방 입장 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room Successful...!");
        //FirebaseManager.Instance.UpdatePlayerCount(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount);
    }

    // 방 나가기 콜백
    public override void OnLeftRoom()
    {
        Debug.Log("On Left Room...!");
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
}
