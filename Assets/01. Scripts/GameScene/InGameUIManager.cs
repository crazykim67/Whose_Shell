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

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
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
}
