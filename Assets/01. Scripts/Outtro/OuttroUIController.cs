using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OuttroUIController : MonoBehaviour
{
    [SerializeField]
    private PhotonView pv;

    [SerializeField]
    private GameObject dimObj;

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

    private void Awake() => roomSettingUI = FindAnyObjectByType<RoomSetting>();

    public IEnumerator ShowOuttroSequence(int type)
    {
        if (FadeController.Instance != null)
            FadeController.Instance.OnFadeIn(0.5f, () => 
            {
                dimObj.SetActive(true);
                turtleObj.SetActive(true);
            });

        ShowPlayer(type);

        yield return new WaitForSeconds(3f);

        OnGameSetting();

        if (FadeController.Instance != null)
            FadeController.Instance.OnFadeIn(0.5f, () =>
            {
                if (GameSystem.Instance != null)
                    GameSystem.Instance.OnEnd();

                OnInit();
                MapManager.Instance.MapIndex(0);
                dimObj.SetActive(false);
                turtleObj.SetActive(false);
            });
    }

    public void OnGameSetting()
    {
        if(roomSettingUI != null)
            roomSettingUI.SetActive(true);

        if (CustomManager.Instance != null)
            CustomManager.Instance.SetActive(true);
    }

    [PunRPC]
    public void ShowOuttroRPC(int type)
    {
        StartCoroutine(ShowOuttroSequence(type));

        if (GameSystem.Instance == null)
            return;

        InGameUIManager.Instance.EndGameSetting();

    }

    public void ShowPlayer(int type)
    {
        if(GameSystem.Instance == null) 
            return;

        var players = GameSystem.Instance.GetPlayerList();

        PlayerController myPlayer = null;

        foreach (var player in players)
        {
            if (player.pv.IsMine)
            {
                myPlayer = player;
                break;
            }
        }

        myCharacter.gameObject.SetActive(false);

        for(int i = 0; i < players.Count; i++)
            otherCharacters[i].gameObject.SetActive(false);

        // ÀÚ¶ó ½Â¸®
        if (type == 1)
        {
            playerType.text = "ÀÚ¶ó ½Â¸®";
            playerType.color = gradientImage.color = terrapinColor;

            if ((myPlayer.playerType & PlayerType.Terrapin) == PlayerType.Terrapin)
            {
                myCharacter.SetIntroCharacter(PhotonNetwork.NickName, myPlayer.playerColor);
                myCharacter.gameObject.SetActive(true);
            }
            else
                myCharacter.gameObject.SetActive(false);

            int i = 0;
            foreach (var player in players)
            {
                if (!player.pv.IsMine && (player.playerType & PlayerType.Terrapin) == PlayerType.Terrapin)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickName, player.playerColor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
        // °ÅºÏÀÌ ½Â¸®
        else
        {
            playerType.text = "°ÅºÏÀÌ ½Â¸®";
            playerType.color = gradientImage.color = turtleColor;

            if ((myPlayer.playerType & PlayerType.Turtle) == PlayerType.Turtle)
                myCharacter.SetIntroCharacter(PhotonNetwork.NickName, myPlayer.playerColor);
            else
                myCharacter.gameObject.SetActive(false);

            int i = 0;
            foreach (var player in players)
            {
                if (!player.pv.IsMine && (player.playerType & PlayerType.Turtle) == PlayerType.Turtle)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickName, player.playerColor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
    }

    // Outtro RPC ½ÇÇà
    // 0 = °ÅºÏÀÌ ½Â¸®, 1 = ÀÚ¶ó ½Â¸®
    public void OnOuttro(int type)
    {
        pv.RPC("ShowOuttroRPC", RpcTarget.All, type);
    }

    public void OnInit()
    {
        if (GameManager.Instance == null)
            return;

        if (GameSystem.Instance == null)
            return;

        if (InGameUIManager.Instance == null)
            return;

        var manager = GameManager.Instance;
        var system = GameSystem.Instance;
        var ui = InGameUIManager.Instance;

        // ÇÃ·¹ÀÌ¾î Å¸ÀÔ ÃÊ±âÈ­
        system.Init();
        ui.TaskUI.Init();

        foreach (var player in system.controllerList)
                player.Init();
    }
}
