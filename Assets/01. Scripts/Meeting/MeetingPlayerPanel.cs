using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class MeetingPlayerPanel : MonoBehaviour
{
    [SerializeField]
    private Image playerImage;

    [SerializeField]
    private TextMeshProUGUI nickNameText;

    [SerializeField]
    private GameObject deadPlayerBlock;
    [SerializeField]
    private GameObject reportSign;

    [SerializeField]
    private GameObject voteButtons;

    [HideInInspector]
    public PlayerController targetPlayer;

    [SerializeField]
    private GameObject voteSign;

    [SerializeField]
    private GameObject voterPrefab;

    [SerializeField]
    private Transform voterParent;

    public void UpdatePanel(float voterHue)
    {
        var voter = Instantiate(voterPrefab, voterParent).GetComponent<Image>();

        voter.material = Instantiate(voter.material);
        voter.material.SetFloat("_Hue", voterHue);
    }

    public void UpdateVoteSign(bool isVoted)
    {
        voteSign.SetActive(isVoted);
    }

    public void OpenResult()
    {
        voterParent.gameObject.SetActive(true);
    }


    public void SetPlayer(PlayerController target)
    {
        Material inst = Instantiate(playerImage.material);
        playerImage.material = inst;

        targetPlayer = target;
        playerImage.material.SetFloat("_Hue", targetPlayer.GetColor());

        PlayerController myCharacter = null;

        nickNameText.text = targetPlayer.nickName;

        foreach (var player in GameSystem.Instance.controllerList)
        {
            if (player.nickName.Equals(PhotonNetwork.NickName))
            {
                myCharacter = player;
                break;
            }
        }

        if((myCharacter.playerType & PlayerType.Terrapin) == PlayerType.Terrapin
            && ((targetPlayer.playerType & PlayerType.Terrapin) == PlayerType.Terrapin))
            nickNameText.color = Color.red;

        bool isDead = (targetPlayer.playerType & PlayerType.Ghost) == PlayerType.Ghost;

        deadPlayerBlock.SetActive(isDead);
        GetComponent<Button>().interactable = !isDead;

        reportSign.SetActive(targetPlayer.isReporter);
    }

    public void OnClickPlayerPanel()
    {
        PlayerController myCharacter = null;


        foreach(var player in GameSystem.Instance.controllerList) 
        {
            if (player.nickName.Equals(PhotonNetwork.NickName))
            {
                myCharacter = player;
                break;
            }
        }

        if (myCharacter.isVote)
            return;

            if ((myCharacter.playerType & PlayerType.Ghost) != PlayerType.Ghost)
        {
            InGameUIManager.Instance.MeetingUI.SelectPlayerPanel();
            voteButtons.SetActive(true);
        }
    }

    public void Select()
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

        myCharacter.VoteEjectPlayer(targetPlayer.playerColor);
        Unselect();
    }

    public void Unselect()
    {
        voteButtons.SetActive(false);
    }
}
