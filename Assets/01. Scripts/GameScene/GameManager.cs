using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

// 게임의 전체적인 규칙을 관리하는 스크립트
public class GameManager : MonoBehaviourPunCallbacks
{
    private PhotonView pv;

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

    public GameRuleData ruleData;

    [HideInInspector]
    public int terrapinCount;

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
    }

    public override void OnJoinedRoom()
    {
        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
        int terrapin = (int)cp["Terrapin"];

        terrapinCount = terrapin;
    }

    public void SetData(int _emCount, int _emCoolTime, int _emTime,
        int _voteTime, float _speed, float _tuSight, float _teSight,
        float _killCoolTime, int _range, int _commonTask, int _simpleTask)
    {
        pv.RPC("SetDataRPC", RpcTarget.AllBuffered, _emCount, _emCoolTime, _emTime, _voteTime,
            _speed, _tuSight, _teSight, _killCoolTime, _range, _commonTask, _simpleTask);
    }

    [PunRPC]
    public void SetDataRPC(int _emCount, int _emCoolTime, int _emTime,
        int _voteTime, float _speed, float _tuSight, float _teSight,
        float _killCoolTime, int _range, int _commonTask, int _simpleTask)
    {
        GameRuleData data = new GameRuleData(_emCount, _emCoolTime, _emTime, _voteTime, 
            _speed, _tuSight, _teSight, _killCoolTime, (KillRange)_range, _commonTask, _simpleTask);

        ruleData = data;
    }
}
