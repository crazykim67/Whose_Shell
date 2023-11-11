using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using System;

public class AuthManager : MonoBehaviour
{
    #region Instance

    private static AuthManager instance;

    public static AuthManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new AuthManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion 

    private Firebase.Auth.FirebaseAuth auth;

    public string currentId;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // 로그인 메서드
    public void OnLogin(string _id, string _email, string _pass)
    {
        auth.SignInWithEmailAndPasswordAsync($"{_id}@{_email}", _pass).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                { 
                    Debug.Log($"{_id} is Login Successful");
                    currentId = _id;
                }
                else if (task.IsFaulted)
                {
                    Debug.Log("Login is Failed...!");
                    return;
                }
            });
    }

    // 회원가입 메서드
    public void OnRegister(string _id, string _email, string _pass)
    {
        auth.CreateUserWithEmailAndPasswordAsync($"{_id}@{_email}", _pass).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                {
                    Debug.Log($"{_id} is Register Successful");
                    FirebaseManager.Instance.OnSaveData(_id, $"{_id}@{_email}");
                }
                else if (task.IsFaulted)
                {
                    Debug.Log("Register is Failed...!");
                    return;
                }
            });
    }

    // 로그아웃 메서드
    public void OnLogOut()
    {
        if (auth.CurrentUser == null)
            return;

        auth.SignOut();
        Debug.Log("Logout Successful");
    }

    public void OnApplicationQuit() => OnLogOut();
}
