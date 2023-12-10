using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSetting : MonoBehaviour
{
    private PhotonView pv;

    public TextMeshProUGUI nickNameText;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        if(pv.IsMine)
            pv.RPC("SetNickName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
    }

    [PunRPC]
    public void SetNickName(string _nick)
    {
        nickNameText.text = _nick;
    }

    public void SetColor(Color color)
    {
        nickNameText.color = color;
    }
}
