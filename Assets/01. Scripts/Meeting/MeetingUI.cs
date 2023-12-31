using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPanelPrefab;

    [SerializeField]
    private Transform playerPanelParent;

    [SerializeField]
    private GameObject voterPrefab;

    [SerializeField]
    private GameObject skipVoteBtn;

    [SerializeField]
    private GameObject skipVoteplayers;

    [SerializeField]
    private Transform skipVoteParent;

    [SerializeField]
    private List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();
    [SerializeField]
    private List<Image> skipPlayer = new List<Image>();

    [SerializeField]
    private TextMeshProUGUI meetingTimeText;

    private bool isStart = false;

    public void OnShow()
    {
        if (GameSystem.Instance == null)
            return;

        isStart = true;

        var players = GameSystem.Instance.GetPlayerList();

        PlayerController myCharacter = null;

        foreach (var player in players)
        {
            if (player.nickName.Equals(PhotonNetwork.NickName))
            {
                myCharacter = player;
                break;
            }
        }

        var myPanel = Instantiate(playerPanelPrefab, playerPanelParent).GetComponent<MeetingPlayerPanel>();
        myPanel.SetPlayer(myCharacter);
        meetingPlayerPanels.Add(myPanel);

        gameObject.SetActive(true);

        foreach(var player in players)
        {
            if(player != myCharacter)
            {
                var panel = Instantiate(playerPanelPrefab, playerPanelParent).GetComponent<MeetingPlayerPanel>();
                panel.SetPlayer(player);
                meetingPlayerPanels.Add(panel);
            }
        }
    }

    public void SelectPlayerPanel()
    {
        foreach(var panel in meetingPlayerPanels)
        {
            panel.Unselect();
        }
    }

    public void UpdateVote(float voterColor, float ejectColor)
    {
        foreach(var panel in meetingPlayerPanels)
        {
            if(panel.targetPlayer.playerColor == ejectColor)
            {
                panel.UpdatePanel(voterColor);
            }

            if(panel.targetPlayer.playerColor == voterColor)
            {
                panel.UpdateVoteSign(true);
            }
        }
    }

    public void UpdateSkipVotePlayer(float hue)
    {
        foreach(var panel in meetingPlayerPanels)
        {
            if(panel.targetPlayer.playerColor == hue)
            {
                panel.UpdateVoteSign(true);
            }
        }

        var voter = Instantiate(voterPrefab, skipVoteParent).GetComponent<Image>();

        voter.material = Instantiate(voter.material);
        voter.material.SetFloat("_Hue", hue);

        skipPlayer.Add(voter);
    }

    public void OnClickSkip()
    {
        PlayerController myCharacter = null;

        foreach (var player in GameSystem.Instance.controllerList)
        {
            if (player.nickName.Equals(PhotonNetwork.NickName))
            {
                myCharacter = player;
                break;
            }
        }

        if (myCharacter.isVote)
            return;

        if ((myCharacter.playerType & PlayerType.Ghost) == PlayerType.Ghost)
            return;

        myCharacter.SkipVote();
        SelectPlayerPanel();
    }

    // 최종적 투표 끝
    public void OpenResult()
    {
        foreach(var panel in meetingPlayerPanels)
        {
            panel.OpenResult();
        }

        skipVoteplayers.SetActive(true);
        skipVoteBtn.SetActive(false);
        isStart = false;
    }

    private void Update()
    {
        if (isStart)
        {
            meetingTimeText.text = string.Format("회의시간 : {0}s", (int)Mathf.Clamp(GameSystem.Instance.remainTime, 0f, float.MaxValue));
        }
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
        skipVoteplayers.SetActive(false);
        skipVoteBtn.SetActive(true);

        foreach(var panel in meetingPlayerPanels)
            Destroy(panel.gameObject);

        foreach (var player in skipPlayer)
            Destroy(player.gameObject);

        foreach(var player in GameSystem.Instance.controllerList)
            player.SetReport(false);

        meetingPlayerPanels.Clear();
        skipPlayer.Clear();
    }
}
