using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Playables;
using NUnit.Framework;


public class GameSystem : MonoBehaviourPunCallbacks
{
    #region Instance

    private static GameSystem instance;

    public static GameSystem Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameSystem();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    private PhotonView pv;

    public List<Player> playerList = new List<Player>();
    public List<PlayerController> controllerList = new List<PlayerController>();

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
        AddPlayer();
    }

    #region Player

    // 처음 접속 시 마스터 클라이언틍 용
    private void AddPlayer()
    {
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerList(player.Value);
        }
    }

    public void AddPlayerList(Player player)
    {
        if (player != null)
            playerList.Add(player);
    }

    public void RemovePlayer(int index)
    {
        playerList.RemoveAt(index);
    }

    #endregion

    #region PlayerController

    public void AddController(PlayerController controller)
    {
        if (controller == null)
            return;

            controllerList.Add(controller);
    }

    private void RemoveController(PlayerController controller)
    {
        if (controller == null)
            return;

        controllerList.Remove(controller);
    }

    #endregion

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerList.FindIndex(x => x == otherPlayer);

        if (index != -1)
            RemovePlayer(index);

        foreach (var controller in controllerList)
        {
            if (controller.nickName.Equals(otherPlayer.NickName))
            {
                RemoveController(controller);
                break;
            }
        }
    }
}
