using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomListController : MonoBehaviourPunCallbacks
{
    private PhotonView pv;

    public PlayerState playerState;

    public Transform tr;

    public List<PlayerState> playerList = new List<PlayerState>();

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        GetCurrentPlayers();
    }

    private void GetCurrentPlayers()
    {
        foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players) 
        {
            AddPlayerList(player.Value);
        }
    }

    public void AddPlayerList(Player player)
    {
        PlayerState _player = Instantiate(playerState, tr);
        if (_player != null)
        {
            _player.SetPlayer(player);
            playerList.Add(_player);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_player.GetComponent<RectTransform>());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerList.FindIndex(x => x.player == otherPlayer);

        if(index != -1)
        {
            Destroy(playerList[index].gameObject);
            playerList.RemoveAt(index);
        }
    }
}
