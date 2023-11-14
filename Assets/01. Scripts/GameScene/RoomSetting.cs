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

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.InRoom)
            return;

        if (PhotonNetwork.IsMasterClient)
        {
            roomCodeText.text = $"Room Code : {PhotonNetwork.CurrentRoom.Name}";
            startBtn.gameObject.SetActive(true);
        }
        else
        {
            roomCodeText.enabled = false;
            startBtn.gameObject.SetActive(false);
        }
    }

    public void OnStart()
    {
        if (GameManager.Instance == null)
            return;

        if (!PhotonNetwork.InRoom)
            return;

        if(GameManager.Instance.terrapinCount == 1)
        {
            Debug.Log("최소인원 4명");
        }
        else if(GameManager.Instance.terrapinCount == 2)
        {
            Debug.Log("최소인원 7명");
        }
        else
        {
            Debug.Log("최소인원 9명");
        }
    }
}
