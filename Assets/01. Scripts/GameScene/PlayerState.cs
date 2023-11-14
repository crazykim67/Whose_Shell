using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerState : MonoBehaviour
{
    public TextMeshProUGUI nickNameText;

    public Player player { get; private set; }

    public void SetPlayer(Player _player)
    {
        player = _player;
        nickNameText.text = player.NickName;
    }
}
