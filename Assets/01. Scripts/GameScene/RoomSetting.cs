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
}
