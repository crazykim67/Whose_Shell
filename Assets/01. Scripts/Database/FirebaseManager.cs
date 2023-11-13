using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Photon.Pun;
using Photon.Realtime;

public class FirebaseManager : MonoBehaviour
{
    #region Instance

    private static FirebaseManager instance;

    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseManager();
                return instance;
            }
            return instance;

        }
    }

    #endregion

    [System.Serializable]
    public class UserData
    {
        public string id;
        public string email;
        public string password;

        public UserData(string _id, string _email, string _password)
        {
            id = _id;
            email = _email;
            password = _password;
        }
    }

    [System.Serializable]
    public class RoomData
    {
        public string roomName;
        public int playerCount;

        public RoomData(string _roomName, int _playerCount)
        {
            roomName = _roomName;
            playerCount = _playerCount;
        }   
    }

    private DatabaseReference databaseReference;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start() => databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

    #region ID, EMAIL DATA

    public void OnSaveData(string id, string email, string password)
    {
        UserData _user = new UserData(id, email, password);
        string jsonData = JsonUtility.ToJson(_user);

        databaseReference.Child("User").Child(id).SetRawJsonValueAsync(jsonData);
    }

    public void OnLoadData()
    {
        databaseReference.Child(AuthManager.Instance.currentId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
                Debug.Log("Load Canceled...!");
            else if (task.IsFaulted)
                Debug.Log("Load Failed...!");
            else
            {
                var _data = task.Result;

                string dataString = "";
                foreach (var data in _data.Children)
                {
                    dataString += $"ID : {data.Key} - Email : {data.Value}\n"; ;
                    Debug.Log(dataString);
                }
            }
        });
    }

    public void CheckID(string _email, string _pass)
    {
        bool isLogin = false;

        databaseReference.Child(AuthManager.Instance.currentId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
                Debug.Log("Load Canceled...!");
            else if (task.IsFaulted)
                Debug.Log("Load Failed...!");
            else
            {
                var _data = task.Result;

                string dataString = "";
                foreach (var data in _data.Children)
                {
                    dataString += $"ID : {data.Key} - Email : {data.Value}\n";
                    Debug.Log(dataString);
                }

                StartCoroutine(CircleTransitionController.Instance.Text());

            }
        });
    }

    #endregion

    #region ROOM DATA

    public void OnRoomData(string _roomName, int _playerCount)
    {
        RoomData _room= new RoomData(_roomName, _playerCount);
        string jsonData = JsonUtility.ToJson(_room);

        databaseReference.Child("RoomList").Child(_roomName).SetRawJsonValueAsync(jsonData);
    }

    public void UpdatePlayerCount(string _roomName, int _playerCount)
    {
        databaseReference.Child("RoomList").Child(_roomName).Child("playerCount").SetValueAsync(_playerCount);
    }


    public void RemoveRoomData(string _roomName)
    {
        databaseReference.Child("RoomList").Child(_roomName).RemoveValueAsync();
    }

    #endregion

}
