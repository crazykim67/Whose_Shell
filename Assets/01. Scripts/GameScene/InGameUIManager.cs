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

    public int commonTaskCount = 0;
    public int simpleTaskCount = 0;

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SetRPCTaskAmount()
    {
        int taskEssential = (PhotonNetwork.CurrentRoom.PlayerCount - GameManager.Instance.terrapinCount);

        if (PhotonNetwork.InRoom)
        {
            taskAmount = 1f / (commonTaskCount + simpleTaskCount);
            commonTaskCount = (taskEssential * GameManager.Instance.ruleData.commonTask);
            simpleTaskCount = (taskEssential * GameManager.Instance.ruleData.simpleTask);
        }
    }

    public void SetTaskAmount()
    {
        pv.RPC("SetRPCTaskAmount", RpcTarget.All);
    }

    public void OnSet(PlayerController player)
    {
        minimap.SetPlayer(player);
    }

    public void StartGameSetting()
    {
        this.uiObject.SetActive(true);
        StartCoroutine(TaskUI.Rebuilder());
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

    [PunRPC]
    public void TaskTypeRpcCheck(int type)
    {
        TaskUI.TaskTypeCheck((TaskType)type);
    }

    public void TaskTypeCheck(int type)
    {
        pv.RPC("TaskTypeRpcCheck", RpcTarget.All, type);
    }

    #endregion
}
