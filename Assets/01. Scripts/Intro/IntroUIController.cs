using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class IntroUIController : MonoBehaviour
{
    [SerializeField]
    private PhotonView pv;

    [SerializeField]
    private GameObject dimObj;

    [SerializeField]
    private GameObject introductObj;

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

    private RoomSetting roomSettingUI;

    [Header("INTRODUCT TEXT")]
    public IntroductText introductText;

    private void Awake() => roomSettingUI = FindAnyObjectByType<RoomSetting>();

    public IEnumerator ShowIntroSequence()
    {
        OnGameSetting(false);
        dimObj.SetActive(true);

        introductObj.SetActive(true);
        introductText.SetAnim(true);

        yield return new WaitForSeconds(3f);

        if (FadeController.Instance != null)
            FadeController.Instance.OnFadeOut(0.5f);

        introductObj.SetActive(false);
        introductText.SetAnim(false);

        ShowPlayer();
        turtleObj.SetActive(true);

        yield return new WaitForSeconds(3f);

        if (FadeController.Instance != null)
            FadeController.Instance.OnFadeIn(0.5f, () => { 
                MapManager.Instance.MapIndex(1); 
                dimObj.SetActive(false);
                turtleObj.SetActive(false);
            }) ;
    }

    public void OnGameSetting(bool isAct)
    {
        if(roomSettingUI != null)
            roomSettingUI.SetActive(isAct);

        if(CustomManager.Instance != null)
            CustomManager.Instance.SetActive(isAct);
    }

    #region RPC

    [PunRPC]
    public void ShowIntroRPC()
    {
        StartCoroutine(ShowIntroSequence());
    }

    #endregion

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

    // Intro RPC 실행
    public void OnIntro()
    {
        pv.RPC("ShowIntroRPC", RpcTarget.All);
    }
}
