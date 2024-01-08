using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TaskScript : MonoBehaviour
{
    public Animator textAnim;

    public Button closeBtn;

    public bool isSuccess = false;

    public PlayerController myCharacter;

    public TaskType taskType = TaskType.None;

    public virtual void Init()
    {
        if (GameSystem.Instance == null)
            return;

        if (myCharacter == null)
            foreach (var player in GameSystem.Instance.controllerList)
            {
                if (player.nickName.Equals(PhotonNetwork.NickName))
                {
                    myCharacter = player;
                    break;
                }
            }

        if(textAnim.gameObject.activeSelf)
            textAnim.SetBool("isSuccess", false);

        textAnim.gameObject.SetActive(false);
    }

    public virtual void OnShow()
    {
        if (isSuccess)
            return;

        Init();
        myCharacter.isUI = true;
        this.gameObject.SetActive(true);
    }

    public virtual void OnHide()
    {
        myCharacter.isUI = false;
        Debug.Log("isUi : " + myCharacter.isUI);
        this.gameObject.SetActive(false);

        if (InGameUIManager.Instance == null)
            return;

        InGameUIManager.Instance.TaskUI.OnHideTaskMission();
    }
}
