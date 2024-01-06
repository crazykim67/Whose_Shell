using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    #region Instance

    private static InGameUIManager instance;

    public static InGameUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InGameUIManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    private PhotonView pv;

    [SerializeField]
    private GameObject uiObject;

    [SerializeField]
    private MinimapManager minimap;

    [SerializeField]
    private KillButtonUI killUI;

    public  KillButtonUI KillUI {get { return killUI; } }

    [SerializeField]
    private ReportButtonUI reportButtonUI;
    public ReportButtonUI ReportButtonUI { get { return reportButtonUI; } }

    [SerializeField]
    private ReportUI reportUI;
    public ReportUI ReportUI { get { return reportUI; } }

    [SerializeField]
    private MeetingUI meetingUI;
    public MeetingUI MeetingUI { get { return meetingUI; } }

    [SerializeField]
    private EjectionUI ejectionUI;
    public EjectionUI EjectionUI { get { return ejectionUI; } }

    [SerializeField]
    private TaskUI taskUI;
    public TaskUI TaskUI { get { return taskUI; } }

    public float taskAmount;

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SetRPCTaskAmout()
    {
        if (PhotonNetwork.InRoom)
            taskAmount = 1f / (PhotonNetwork.CurrentRoom.PlayerCount - GameManager.Instance.terrapinCount) / (GameManager.Instance.ruleData.commonTask + GameManager.Instance.ruleData.simpleTask);
    }

    public void SetTaskAmout()
    {
        pv.RPC("SetRPCTaskAmout", RpcTarget.All);
    }

    public void OnSet(PlayerController player)
    {
        minimap.SetPlayer(player);
    }

    public void StartGameSetting()
    {
        this.uiObject.SetActive(true);
    }

    public void EndGameSetting()
    {
        this.uiObject.SetActive(false);
    }


    #region Task

    public void OnTask(Task task)
    {
        TaskUI.OnTaskMission(task);
    }

    [PunRPC]
    public void OnRpcSuccess()
    {
        TaskUI.OnSuccess();
    }

    public void OnSuccess()
    {
        pv.RPC("OnRpcSuccess", RpcTarget.All);
    }

    #endregion
}
