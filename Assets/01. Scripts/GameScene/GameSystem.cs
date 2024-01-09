using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Rendering.Universal;

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
    public bool isStart = false;

    [Header("Rule Data")]
    public float killCooldown;
    public int killRange;
    public float speed;

    [SerializeField]
    private Light2D shadowLight;

    [SerializeField]
    private Light2D globalLight;

    public int skipVotePlayerCount;

    [Header("Vote Timer")]
    public float remainTime;

    private void Awake()
    {
        instance = this;
        pv = GetComponent<PhotonView>();
        shadowLight = Camera.main.GetComponentInChildren<Light2D>();

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

    #region Photon Callbacks

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

    #endregion

    // 자라 선정
    private IEnumerator GameReady()
    {
        var manager = GameManager.Instance;

        while (PhotonNetwork.CurrentRoom.PlayerCount != controllerList.Count)
            yield return null;

        for(int i = 0; i < manager.terrapinCount; i++)
        {
            int index = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
            var player = controllerList[index];

            if ((player.playerType & PlayerType.Terrapin) != PlayerType.Terrapin)
                pv.RPC("SetPlayerType", RpcTarget.All, player.nickName);
            else
                i--;
        }

        yield return new WaitForSeconds(1f);

        // 모든 사용자에게 거북이/자라 선별 화면 호출 동기화
        IntroGameUIManager.Instance.Controller.OnIntro();
        pv.RPC("SetTerrapinValue", RpcTarget.All);
    }

    #region Start / End

    public void OnStart()
    {
        if (isStart)
            return;

        StartCoroutine(GameReady());
        pv.RPC("IsStart", RpcTarget.All, true);
    }

    public void OnEnd()
    {
        if (!isStart)
            return;

        pv.RPC("IsStart", RpcTarget.All, false);
    }

    #endregion

    #region RPC

    // 해당 플레이어 자라 설정 및 이름표 설정
    [PunRPC]
    private void SetPlayerType(string nickName)
    {
        var manager = GameManager.Instance;
        speed = manager.ruleData.playerSpeed;

        foreach(var controller in controllerList)
            controller.speed = speed;

        // 킬 쿨타임 세팅
        killCooldown = manager.ruleData.killCoolTime;
        killRange = (int)manager.ruleData.killRange;

        foreach (var controller in controllerList)
        {
            if (controller.nickName.Equals(nickName))
            {
                controller.playerType = PlayerType.Terrapin;
                controller.SetTerrapinUI();
                break;
            }
        }
    }

    [PunRPC]
    public void IsStart(bool _isStart)
    {
        isStart = _isStart;

        if (InGameUIManager.Instance != null)
            InGameUIManager.Instance.SetTaskAmount();
    }

    [PunRPC]
    private void SetTerrapinValue()
    {
        foreach (var player in controllerList)
        {
            player.SetKillCooldown(killCooldown);
            player.playerFinder.SetKillRange(killRange + 1f);
            player.SetLight();
        }

        if(VivoxManager.Instance != null)
            VivoxManager.Instance.JoinChannel(PhotonNetwork.CurrentRoom.Name, VivoxUnity.ChannelType.NonPositional);
    }

    #endregion

    public void SetTerrapinUI()
    {
        PlayerController myController = null;

        foreach(var controller in controllerList)
        {
            if (controller.nickName.Equals(PhotonNetwork.NickName))
            {
                myController = controller;
                break;
            }
        }

        if (myController.playerType == PlayerType.Terrapin)
        {
            foreach (var controller in controllerList)
            {
                if(controller.playerType == PlayerType.Terrapin)
                    controller.playerSet.SetColor(Color.red);
                else
                    controller.playerSet.SetColor(Color.white);
            }
        }
        else
        {
            foreach (var controller in controllerList)
                controller.playerSet.SetColor(Color.white);
        }
    }

    #region Init

    public void InitNickName()
    {
        foreach (var controller in controllerList)
        {
            controller.playerSet.SetColor(Color.white);
            controller.playerSet.SetActive(true);
        }
    }

    #endregion

    public List<PlayerController> GetPlayerList()
    {
        return controllerList;
    }

    public void ChangeLightMode(PlayerType type)
    {
        if (shadowLight == null)
            return;

        if(type == PlayerType.Ghost)
        {
            shadowLight.intensity = 0f;
            globalLight.intensity = 1f;
        }
        else
        {
            shadowLight.intensity = 0.5f;
            globalLight.intensity = 0.5f;
        }
    }

    public void StartReportMeeting(float deadbodyColor)
    {
        pv.RPC("RpcSendReportSign", RpcTarget.All, deadbodyColor);
    }

    [PunRPC]
    public void RpcSendReportSign(float deadbodyColor)
    {
        foreach(var player in controllerList) 
        { 
            if (player.pv.IsMine)
            {
                if (InGameUIManager.Instance.TaskUI.isPlaying)
                {
                    InGameUIManager.Instance.TaskUI.CoralCut.OnHide();
                    InGameUIManager.Instance.TaskUI.TrashTask.OnHide();
                    InGameUIManager.Instance.TaskUI.CleanShellTask.OnHide();
                    InGameUIManager.Instance.TaskUI.SpiderWebTask.OnHide();
                    InGameUIManager.Instance.TaskUI.DishTask.OnHide();
                }
            }
        }

        InGameUIManager.Instance.ReportUI.OnShow(deadbodyColor);

        StartCoroutine(StartMeeting_Coroutine());
        StartCoroutine(MeetingProcess_Coroutine());
    }

    private IEnumerator StartMeeting_Coroutine()
    {
        yield return new WaitForSeconds(3f);

        InGameUIManager.Instance.ReportUI.OnHide();
        InGameUIManager.Instance.MeetingUI.OnShow();
    }

    // 자신이 투표를 했는지 누구에게 투표했는지 체크하는 메소드
    public void RpcSignVoteEject(float voterColor, float ejectColor)
    {
        InGameUIManager.Instance.MeetingUI.UpdateVote(voterColor, ejectColor);
    }

    public void RpcSignSkipVote(float hue)
    {
        InGameUIManager.Instance.MeetingUI.UpdateSkipVotePlayer(hue);
    }

    private IEnumerator MeetingProcess_Coroutine()
    {
        skipVotePlayerCount = 0;

        var players = GetPlayerList();

        // 투표 초기화 부분
        foreach(var player in players)
        {
            if((player.playerType & PlayerType.Ghost) != PlayerType.Ghost)
            {
                player.isVote = false;
            }

            player.vote = 0;
        }

        var manager = GameManager.Instance;

        remainTime = manager.ruleData.voteTime;

        while (true)
        {
            remainTime -= Time.deltaTime;
            yield return null;
            if(remainTime <= 0f)
                break;
        }

        foreach(var player in controllerList)
        {
            if(!player.isVote & (player.playerType & PlayerType.Ghost) != PlayerType.Ghost)
            {
                player.isVote = true;
                skipVotePlayerCount += 1;
                RpcSignSkipVote(player.playerColor);
            }
        }

        RpcEndVoteTime();

        yield return new WaitForSeconds(3f);

        StartCoroutine(CalculateVoteResult_Coroutine(players.ToArray()));
    }

    private IEnumerator CalculateVoteResult_Coroutine(PlayerController[] players)
    {
        System.Array.Sort(players, new PlayerVoteComparer());

        int remainTerrapin = 0;

        foreach (var player in players)
            if ((player.playerType & PlayerType.Terrapin) == PlayerType.Terrapin)
                remainTerrapin++;

        // 스킵 투표 수가 플레이어 투표 수 보다 많을 시
        if (skipVotePlayerCount >= players[0].vote)
        {
            RpcShowEjectionUI(false, 0, false, remainTerrapin);
        }
        // 투표 수가 1순위인 플레이어와 2순위인 플레이어가 같을 경우
        else if (players[0].vote == players[1].vote)
        {
            RpcShowEjectionUI(false, 0, false, remainTerrapin);
        }
        // 스킵 투표 수와 2순위 투표 수 플레이어보다 많을 경우
        else
        {
            bool isTerrapin = (players[0].playerType & PlayerType.Terrapin) == PlayerType.Terrapin;
            RpcShowEjectionUI(true, players[0].playerColor, isTerrapin, isTerrapin ? remainTerrapin - 1 : remainTerrapin);

            players[0].isDead(true);
        }

        var deadbodies = FindObjectsByType<Deadbody>(FindObjectsSortMode.None);

        for (int i = 0; i < deadbodies.Length; i++)
        {
            Destroy(deadbodies[i].gameObject);
        }

        if (MapManager.Instance != null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                var spawnPos = MapManager.Instance.SpawnArray()[i].position;

                MapManager.Instance.SetRpcPosition(i, spawnPos.x, spawnPos.y, spawnPos.z);
            }
        }

        yield return new WaitForSeconds(6f);

        CloseEjectUI();
    }

    public void RpcShowEjectionUI(bool isEjection, float _hue, bool isTerrapin, int remainTerrapin)
    {
        InGameUIManager.Instance.EjectionUI.OnShow(isEjection, _hue, isTerrapin, remainTerrapin);
        InGameUIManager.Instance.MeetingUI.OnHide();
    }

    public void CloseEjectUI()
    {
        InGameUIManager.Instance.EjectionUI.OnHide();

        PlayerController myCharacter = null;

        foreach(var player in controllerList)
        {
            if (player.nickName.Equals(PhotonNetwork.NickName))
            {
                myCharacter = player;
                break;
            }
        }

        myCharacter.isUI = false;
    }

    public void RpcEndVoteTime()
    {
        InGameUIManager.Instance.MeetingUI.OpenResult();
    }

    public void Init()
    {
        InitNickName();

        var deadbodies = FindObjectsByType<Deadbody>(FindObjectsSortMode.None);

        if (deadbodies.Length <= 0)
            return;

        for (int i = 0; i < deadbodies.Length; i++)
            Destroy(deadbodies[i].gameObject);
    }

    private class PlayerVoteComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            PlayerController xPlayer = (PlayerController)x;
            PlayerController yPlayer = (PlayerController)y;
            return xPlayer.vote <= yPlayer.vote ? 1 : -1;
        }
    }
}
