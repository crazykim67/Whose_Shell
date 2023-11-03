using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class TestDatabase : MonoBehaviour
{
    #region Instance

    private static TestDatabase instance;

    public static TestDatabase Instance
    {
        get { 
            if(instance == null)
            {
                instance = new TestDatabase();
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

        public UserData(string id, string email)
        {
            this.id = id;
            this.email = email;
        }
    }

    private DatabaseReference databaseReference;

    private void Awake() => instance = this;

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void OnSaveData(string id, string email)
    {
        UserData _user = new UserData(id, email);
        string jsonData = JsonUtility.ToJson(_user);

        databaseReference.Child("User").Child(id).SetRawJsonValueAsync(jsonData);
    }

    public void OnLoadData()
    {
        databaseReference.Child(AuthManager.Instance.currentId).GetValueAsync().ContinueWith(task => 
        {
            if (task.IsCanceled)
                Debug.Log("Load Canceled...!");
            else if(task.IsFaulted) 
                Debug.Log("Load Failed...!");
            else
            {
                var _data = task.Result;

                string dataString = "";
                foreach(var data in _data.Children)
                {
                    dataString += $"ID : {data.Key} - Email : {data.Value}\n";;
                    Debug.Log(dataString);
                }
            }
        });
    }
}
