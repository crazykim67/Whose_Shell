using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class IntroUIController : MonoBehaviour
{
    [SerializeField]
    private PhotonView pv;

    [SerializeField]
    private GameObject introingObj;

    [SerializeField]
    private GameObject turtleObj;

    [SerializeField]
    private TextMeshProUGUI playerType;

    [SerializeField]
    private Image gradientImage;

    [SerializeField]
    private IntroCharacter myCharacter;

    [SerializeField]
    private List<IntroCharacter> otherCharacters = new List<IntroCharacter>();

    [SerializeField]
    private Color turtleColor;

    [SerializeField]
    private Color terrapinColor;

    public IEnumerator ShowIntroSequence()
    {
        this.gameObject.SetActive(true);
        introingObj.SetActive(true);
        yield return new WaitForSeconds(3f);
        introingObj.SetActive(false);

        ShowPlayer();

        turtleObj.SetActive(true);
    }

    public void ShowPlayer()
    {
        var players = GameSystem.Instance.GetPlayerList();

        PlayerController myPlayer = null;

        foreach(var player in players)
        {
            if (player.pv.IsMine)
            {
                myPlayer = player;
                break;
            }
        }

        myCharacter.SetIntroCharacter(PhotonNetwork.NickName, myPlayer.playerColor);

        if(myPlayer.playerType == PlayerType.Terrapin)
        {
            playerType.text = "자라";
            playerType.color = gradientImage.color = terrapinColor;

            int i = 0;
            foreach(var player in players)
            {
                if(!player.pv.IsMine && player.playerType == PlayerType.Terrapin) 
                {
                    otherCharacters[i].SetIntroCharacter(player.nickName, player.playerColor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
        else
        {
            playerType.text = "거북이";
            playerType.color = gradientImage.color = turtleColor;

            int i = 0;
            foreach (var player in players)
            {
                if (!player.pv.IsMine)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickName, player.playerColor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
    }
}
