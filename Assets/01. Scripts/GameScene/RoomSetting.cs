using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomSetting : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomCodeText;

    public Button startBtn;
    public TextMeshProUGUI errorText;

    private bool isError;
    private float timer;

    private void Update()
    {
        if (isError)
        {
            if(timer <= 3)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                isError = false;
                errorText.enabled = false;
            }
        }
    }

    public override void OnJoinedRoom()
    {
        MasterCheck();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        MasterCheck();
    }

    public void MasterCheck()
    {
        if (!PhotonNetwork.InRoom)
            return;

        if (PhotonNetwork.IsMasterClient)
        {
            roomCodeText.text = $"Room Code : {PhotonNetwork.CurrentRoom.Name}";
            startBtn.gameObject.SetActive(true);
            errorText.gameObject.SetActive(true);
        }
        else
        {
            roomCodeText.enabled = false;
            startBtn.gameObject.SetActive(false);
            errorText.gameObject.SetActive(false);
        }
    }

    public void OnStart()
    {
        if (GameManager.Instance == null)
            return;

        if (!PhotonNetwork.InRoom)
            return;

        if(GameManager.Instance.terrapinCount == 1)
            OnError("게임 인원이 최소 4명 이상이어야 합니다.");
        else if(GameManager.Instance.terrapinCount == 2)
            OnError("게임 인원이 최소 7명 이상이어야 합니다.");
        else
            OnError("게임 인원이 최소 9명 이상이어야 합니다.");
    }

    public void OnError(string str)
    {
        isError = true;
        errorText.text = str;
        errorText.enabled = true;
    }
}
