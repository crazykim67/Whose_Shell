using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System;
using VivoxUnity;
using Photon.Pun;

[Serializable]
public class Vivox
{
    public VivoxUnity.Client client;

    public Uri server = new Uri("https://unity.vivox.com/appconfig/6c6d1-whose-92719-udash");
    public string issuer = "6c6d1-whose-92719-udash";
    public string domain = "mtu1xp.vivox.com";
    public string tokenKey = "L7XRwfPL9dUwXpxp8mCpRMYMNCHOHyEL";
    public TimeSpan timespan = TimeSpan.FromSeconds(90);

    public ILoginSession loginSession;
    public IChannelSession channelSession;

    public IAudioDevices audioInputDevice;
    public IAudioDevices audioOutputDevice;
}

public class VivoxManager : MonoBehaviour
{
    public Vivox vivox = new Vivox();

    #region Instance

    private static VivoxManager instance;

    public static VivoxManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new VivoxManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        vivox.client = new VivoxUnity.Client();
        vivox.client.Uninitialize();
        vivox.client.Initialize();
        SetAudioDevices();
    }

    public void Login(string userName)
    {
        AccountId accoundId = new AccountId(vivox.issuer, userName, vivox.domain);
        vivox.loginSession = vivox.client.GetLoginSession(accoundId);

        // 공식 문서에서 TryCatch 사용이 무난하다고 함.
        vivox.loginSession.BeginLogin(vivox.server, vivox.loginSession.GetLoginToken(vivox.tokenKey, vivox.timespan),
            callback =>
            {
                try
                {
                    vivox.loginSession.EndLogin(callback);
                    Debug.Log($"Vivox LoginSession Login Successful...!\n ID : {userName}");
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    throw;
                }
            });
    }

    public void JoinChannel(string channelName, ChannelType channelType)
    {
        ChannelId channelId = new ChannelId(vivox.issuer, channelName, vivox.domain, channelType);
        vivox.channelSession = vivox.loginSession.GetChannelSession(channelId);

        UserCallBacks(true, vivox.channelSession);
        vivox.channelSession.BeginConnect(true, true, true, vivox.channelSession.GetConnectToken(vivox.tokenKey, vivox.timespan), callback =>
        {
            try
            {
                vivox.channelSession.EndConnect(callback);
                Debug.Log($"Vivox ChannelSession Join Successful...!\n Channel Name : {channelName}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        });
    }

    #region 사용자 참여, 나가기 관련 콜백

    // 사용자 참여, 나가기 콜백
    public void UserCallBacks(bool bind, IChannelSession session)
    {
        if (bind)
        {
            vivox.channelSession.Participants.AfterKeyAdded += AddUser;
            vivox.channelSession.Participants.BeforeKeyRemoved += LeaveUser;
        }
        else
        {
            vivox.channelSession.Participants.AfterKeyAdded -= AddUser;
            vivox.channelSession.Participants.BeforeKeyRemoved -= LeaveUser;
        }
    }

    // 사용자 추가
    public void AddUser(object sender, KeyEventArg<string> userData)
    {
        var temp = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;

        IParticipant user = temp[userData.Key];
    }

    // 사용자 나감
    public void LeaveUser(object sender, KeyEventArg<string> userData)
    {
        var temp = (VivoxUnity.IReadOnlyDictionary<string, IParticipant>)sender;

        IParticipant user = temp[userData.Key];
    }

    #endregion

    #region 오디오 관련

    // 오디오 장치 세팅
    public void SetAudioDevices(/*IAudioDevice targetInput = null, IAudioDevice targetOutput = null*/)
    {
#if UNITY_ANDROID || UNITY_IOS
        if(!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Microphone))
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Microphone);
#endif

        vivox.audioInputDevice = vivox.client.AudioInputDevices;
        vivox.audioOutputDevice = vivox.client.AudioOutputDevices;
    }

    public void MuteOtherUser(IParticipant _user, bool isMute)
    {
        if (vivox.channelSession == null)
            return;

        // sip 세션 프로토콜
        string constructedParticipantKey = "sip:." + vivox.issuer + "." + _user.Account.Name + ".@" + vivox.domain;
        var participants = vivox.channelSession.Participants;

        if (participants[constructedParticipantKey].InAudio)
            participants[constructedParticipantKey].LocalMute = isMute;
        else
            // 상대방 오디오 장치 문제 발견
            Debug.Log("Try Again");
    }

    public void OnInputAudioMute(bool isMute)
    {
        vivox.client.AudioInputDevices.Muted = isMute;
    }

    public void OnOutputAudioMute(bool isMute)
    {
        vivox.client.AudioOutputDevices.Muted = isMute;
    }

    #endregion

    public void OnLeft(string roomName = "")
    {
        if (!roomName.Equals(string.Empty))
        {
            if (vivox.channelSession != null)
            {
                vivox.channelSession.Disconnect();
                vivox.loginSession.DeleteChannelSession(new ChannelId(vivox.issuer, roomName, vivox.domain, ChannelType.NonPositional));
            }
        }

        vivox.client.Uninitialize();
        Debug.Log("Vivox Discconect...!");
    }

}
