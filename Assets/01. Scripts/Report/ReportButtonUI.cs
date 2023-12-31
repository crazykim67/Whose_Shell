using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ReportButtonUI : MonoBehaviour
{
    [SerializeField]
    private Button reportBtn;

    public void SetInteractable(bool isInteract)
    {
        reportBtn.interactable = isInteract;
    }

    public bool IsInteractable()
    {
        if (reportBtn.interactable)
            return true;
        else
            return false;
    }

    public void OnClickButton()
    {
        if (GameSystem.Instance == null)
            return;

        PlayerController myPlayer = null;

        foreach(var player in GameSystem.Instance.controllerList)
        {
            if (player.nickName.Equals(PhotonNetwork.NickName))
            {
                myPlayer = player;
                break;
            }
        }

        myPlayer.Report();
    }
}
