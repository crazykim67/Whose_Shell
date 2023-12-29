using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public enum KillRange
{
    Short, Normal, Long
}

public enum TaskBarUpdates
{
    Always, Meetings, Never
}

[System.Serializable]
public struct GameRuleData
{
    public int emergencyCount;
    public int emergencyCoolTime;
    public int emergencyTime;
    public int voteTime;
    public float playerSpeed;
    public float turtleSight;
    public float terrapinSight;
    public float killCoolTime;
    public KillRange killRange;
    public int commonTask;
    public int simpleTask;

    public GameRuleData(int _emCount, int _emCoolTime, int _emTime, 
        int _voteTime, float _speed, float _tuSight, float _teSight,
        float _killCoolTime, KillRange _range, int _commonTask, int _simpleTask)
    {
        emergencyCount = _emCount;
        emergencyCoolTime = _emCoolTime;
        emergencyTime = _emTime;
        voteTime = _voteTime;
        playerSpeed = _speed;
        turtleSight= _tuSight;
        terrapinSight = _teSight;
        killCoolTime = _killCoolTime;
        killRange = _range;
        commonTask = _commonTask;
        simpleTask = _simpleTask;
    }
}

public class GameRuleSetting : MonoBehaviour
{
    [SerializeField]
    private PhotonView pv;

    private int emergencyCount;
    [SerializeField]
    private TextMeshProUGUI emergencyCountText;

    private int emergencyCoolTime;
    [SerializeField]
    private TextMeshProUGUI emergencyCoolTimeText;

    private int emergencyTime;
    [SerializeField]
    private TextMeshProUGUI emergencyTimeText;

    private int voteTime;
    [SerializeField]
    private TextMeshProUGUI voteTimeText;

    private float playerSpeed;
    [SerializeField]
    private TextMeshProUGUI playerSpeedText;

    private float turtleSight;
    [SerializeField]
    private TextMeshProUGUI turtleSightText;

    private float terrapinSight;
    [SerializeField]
    private TextMeshProUGUI terrapinSightText;

    private float killCoolTime;
    [SerializeField]
    private TextMeshProUGUI killCoolTimeText;

    private KillRange killRange;
    [SerializeField]
    private TextMeshProUGUI killRangeText;
    
    //private TaskBarUpdates taskBarUpdates;

    private int commonTask;
    [SerializeField]
    private TextMeshProUGUI commonTaskText;
    
    private int simpleTask;
    [SerializeField]
    private TextMeshProUGUI simpleTaskText;

    [SerializeField]
    private TextMeshProUGUI gameRuleOverview;

    public void GetPhotonView()
    {
        pv = GetComponent<PhotonView>();
        InitGameRule();
    }

    public void InitGameRule()
    {
        emergencyCount = 2;
        emergencyCoolTime = 20;
        emergencyTime = 20;
        voteTime = 30;
        playerSpeed = 3.5f;
        turtleSight = 8f;
        terrapinSight = 10f;
        killCoolTime = 40f;
        killRange = KillRange.Normal;
        commonTask = 1;
        simpleTask = 2;

        //pv.RPC("SetSettings", RpcTarget.AllBuffered);
        RuleDataSetting(emergencyCount, emergencyCoolTime, emergencyTime, voteTime,
            playerSpeed, turtleSight, terrapinSight, killCoolTime, (int)killRange, commonTask, simpleTask);
    }

    #region Show & Hide

    public void OnShow()
    {
        gameObject.SetActive(true);
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region On Value Changed

    public void OnEmergencyCountChange(bool isPlus)
    {
        emergencyCount = Mathf.Clamp(emergencyCount + (isPlus ? 1 : -1), 0, 9);
        emergencyCountText.text = string.Format("{0}", emergencyCount);
    }

    public void OnEmergencyCoolTimeChange(bool isPlus)
    {
        emergencyCoolTime = Mathf.Clamp(emergencyCoolTime + (isPlus ? 5 : -5), 0, 60);
        emergencyCoolTimeText.text = string.Format("{0}s", emergencyCoolTime);
    }

    public void OnEmergencyTimeChange(bool isPlus)
    {
        emergencyTime = Mathf.Clamp(emergencyTime + (isPlus ? 5 : -5), 0, 120);
        emergencyTimeText.text = string.Format("{0}s", emergencyTime);
    }

    public void OnVoteTimeChange(bool isPlus)
    {
        voteTime = Mathf.Clamp(voteTime + (isPlus ? 5 : -5), 0, 300);
        voteTimeText.text = string.Format("{0}s", voteTime);
    }

    public void OnPlayerSpeedChange(bool isPlus)
    {
        playerSpeed = Mathf.Clamp(playerSpeed + (isPlus ? 0.25f : -0.25f), 5f, 8f);
        playerSpeedText.text = string.Format("{0}", playerSpeed);
    }

    public void OnTurtleSightChange(bool isPlus)
    {
        turtleSight = Mathf.Clamp(turtleSight + (isPlus ? 0.25f : -0.25f), 3f, 10f);
        turtleSightText.text = string.Format("{0}", turtleSight);
    }

    public void OnTerrapinSightChange(bool isPlus)
    {
        terrapinSight = Mathf.Clamp(terrapinSight + (isPlus ? 0.25f : -0.25f), 3f, 10f);
        terrapinSightText.text = string.Format("{0}", terrapinSight);
    }

    public void OnKillCoolTimeChange(bool isPlus)
    {
        killCoolTime = Mathf.Clamp(killCoolTime + (isPlus ? 2.5f : -2.5f), 10f, 60f);
        killCoolTimeText.text = string.Format("{0}s", killCoolTime);
    }

    public void OnKillRangeChange(bool isPlus)
    {
        killRange = (KillRange)Mathf.Clamp((int)killRange + (isPlus ? 1 : -1), 0, 2);
        killRangeText.text = string.Format("{0}", killRange);
    }

    public void OnCommonTaskChange(bool isPlus)
    {
        commonTask = Mathf.Clamp(commonTask + (isPlus ? 1 : -1), 0, 2);
        commonTaskText.text = string.Format("{0}", commonTask);
    }

    public void OnSimpleTaskChange(bool isPlus)
    {
        simpleTask = Mathf.Clamp(simpleTask + (isPlus ? 1 : -1), 0, 3);
        simpleTaskText.text = string.Format("{0}", simpleTask);
    }

    #endregion

    [PunRPC]
    public void SetSettings(int _emCount, int _emCoolTime, int _emTime,
        int _voteTime, float _speed, float _tuSight, float _teSight,
        float _killCoolTime, int _range, int _commonTask, int _simpleTask)
    {
        emergencyCountText.text = string.Format("{0}", _emCount);
        emergencyCoolTimeText.text = string.Format("{0}s", _emCoolTime);
        emergencyTimeText.text = string.Format("{0}s", _emTime);
        voteTimeText.text = string.Format("{0}s", _voteTime);
        playerSpeedText.text = string.Format("{0}", _speed);
        turtleSightText.text = string.Format("{0}", _tuSight);
        terrapinSightText.text = string.Format("{0}", _teSight);
        killCoolTimeText.text = string.Format("{0}s", _killCoolTime);
        killRangeText.text = string.Format("{0}", _range);
        commonTaskText.text = string.Format("{0}", _commonTask);
        simpleTaskText.text = string.Format("{0}", _simpleTask);
    }

    [PunRPC]
    public void RuleDataSetting(int _emCount, int _emCoolTime, int _emTime,
        int _voteTime, float _speed, float _tuSight, float _teSight,
        float _killCoolTime, int _range, int _commonTask, int _simpleTask)
    {
        GameManager.Instance.SetData(_emCount, _emCoolTime, _emTime, _voteTime, _speed,
            _tuSight, _teSight, _killCoolTime, _range, _commonTask, _simpleTask);
    }

    public void OnConfirm()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("SetSettings", RpcTarget.AllBuffered, emergencyCount, emergencyCoolTime, emergencyTime, voteTime,
                playerSpeed, turtleSight, terrapinSight, killCoolTime, (int)killRange, commonTask, simpleTask);
            pv.RPC("RuleDataSetting", RpcTarget.AllBuffered, emergencyCount, emergencyCoolTime, emergencyTime, voteTime,
                playerSpeed, turtleSight, terrapinSight, killCoolTime, (int)killRange, commonTask, simpleTask);
        }

        //RuleDataSetting();
        OnHide();
    }
}
