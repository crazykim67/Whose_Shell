using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

// 0x : 0 - 거북이 1 - 자라
// x0 : 0 - 생존 1 = 사망
// 00 - 생존 거북이 01 생존 자라
// 10 - 죽은 거북이 11 죽은 자라
public enum PlayerType
{
    Turtle = 0,
    Terrapin = 1,
    Ghost = 2,
    Tutle_Alive = 0,
    Terrapin_Alive = 1,
    Turtle_Ghost = 2,
    Terrapin_Ghost = 3,
}

public class PlayerController : MonoBehaviour
{
    [Header("Sprite Sorter")]
    public SortingSprite sorting;

    [SerializeField]
    private Transform spriteTr;

    [HideInInspector]
    public PhotonView pv;

    private float moveX, moveY;

    public float speed = 3f;

    private Camera cam;

    public SpriteRenderer sprite;
    public Material mat;
    public float playerColor;

    public Animator anim;
    private bool isMove = false;

    public bool isUI = false;

    public PlayerType playerType = PlayerType.Turtle;

    public PlayerSetting playerSet;
    public string nickName;

    [Header("Sight")]
    [SerializeField]
    private Light2D shadowLight;

    [Header("Kill")]
    [SerializeField]
    private float killCooldown;
    public float KillCooldown { get { return killCooldown; } }
    public bool isKill { get { return killCooldown < 0f && playerFinder.players.Count != 0; } }
    public PlayerFinder playerFinder;

    [Header("Deadbody")]
    public float deadbodyColor;

    [Header("Report")]
    public bool isReporter = false;

    [Header("Meeting")]
    public bool isVote = false;
    public int vote;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            Init();
            cam = Camera.main.GetComponent<Camera>();
            shadowLight = cam.GetComponentInChildren<Light2D>();

            cam.transform.position = new Vector3(0, -0.25f, -1);
            Material playerMat = new Material(mat);
            sprite.material = playerMat;
            mat = playerMat;
            pv.RPC("SetNickName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
            pv.RPC("SetController", RpcTarget.AllBufferedViaServer);
        }
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            CustomManager.Instance.SetPlayer(this);
            InGameUIManager.Instance.OnSet(this);
            SetInitColor();
        }
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            Move();
            Anim();
            cam.transform.SetParent(this.transform);
        }
    }

    private void Update()
    {
        if(playerType == PlayerType.Terrapin)
            killCooldown -= Time.deltaTime;
    }

    public void Init()
    {
        playerType = PlayerType.Turtle;
    }

    public void Anim()
    {
        anim.SetBool("isMove", isMove);
    }

    public void Move()
    {
        if (isUI)
            return;

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
            isMove = true;
        else
            isMove = false;

        transform.Translate(new Vector2(moveX, moveY).normalized * speed * Time.deltaTime);

        if (moveX < 0f)
            spriteTr.localScale = new Vector3(-1f, 1f, 1f);
        else if (moveX > 0f)
            spriteTr.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnStop()
    {
        moveX = 0;
        moveY = 0;
        isMove = false;
        Anim();
    }

    public void SetTerrapinActive(List<PlayerController > list)
    {
        if (!pv.IsMine)
            return;

        if(playerType == PlayerType.Terrapin)
        {
            foreach (var player in list)
            {
                if(player.playerType == PlayerType.Turtle)
                    player.playerSet.SetActive(false);
            }
        }
        else
        {
            foreach (var player in list)
                player.playerSet.SetActive(false);
        }

        
    }

    #region Photon RPC

    [PunRPC]
    public void SetRPCColor(float _hue)
    {
        sprite.material.SetFloat("_Hue", _hue);
        playerColor = _hue;
    }

    [PunRPC]
    public void SetNickName(string _nick)
    {
        nickName = _nick;
    }

    [PunRPC]
    public void SetController()
    {
        GameSystem.Instance.AddController(this);

        if (GameSystem.Instance == null)
            return;
    }

    [PunRPC]
    public void RpcTeleport(float x, float y, float z)
    {
        this.transform.position = new Vector3(x, y, z);
    }

    [PunRPC]
    public void IsReporter(bool isAct)
    {
        this.isReporter = isAct;
    }

    [PunRPC]
    public void RpcVoteEjectPlayer(float hue)
    {
        isVote = true;

        GameSystem.Instance.RpcSignVoteEject(playerColor, hue);

        var players = GameSystem.Instance.controllerList;

        PlayerController ejectPlayer = null;

        foreach(var player in players)
            if(player.playerColor == hue)
                ejectPlayer = player;

        ejectPlayer.vote += 1;
    }

    public void VoteEjectPlayer(float hue)
    {
        pv.RPC("RpcVoteEjectPlayer", RpcTarget.All, hue);
    }

    [PunRPC]
    public void RpcSkipVote()
    {
        isVote = true;
        GameSystem.Instance.skipVotePlayerCount += 1;
        GameSystem.Instance.RpcSignSkipVote(playerColor);
    }

    public void SkipVote()
    {
        pv.RPC("RpcSkipVote", RpcTarget.All);
    }

    [PunRPC]
    public void RpcInitBtns()
    {
        if (CustomManager.Instance == null)
            return;

        CustomManager.Instance.InitBtns();
    }

    #endregion

    #region Kill && Dead

    public void SetTerrapinUI()
    {
        if (InGameUIManager.Instance == null)
            return;

        if (!pv.IsMine)
            return;

        if(playerType == PlayerType.Terrapin)
            InGameUIManager.Instance.KillUI.OnShow(this);
    }

    public void SetKillCooldown(float _cooldown)
    {
        if (!pv.IsMine)
            return;

        killCooldown = _cooldown;
    }

    public void Kill()
    {
        RPCKill(playerFinder.GetNearTarget().pv.ViewID);
    }

    public void RPCKill(int _viewId)
    {
        PlayerController target = null;
        foreach(var player in GameSystem.Instance.GetPlayerList())
        {
            if(player.pv.ViewID == _viewId)
                target = player;
        }

        if(target != null)
        {
            pv.RPC("RpcTeleport", RpcTarget.All, target.transform.position.x, target.transform.position.y, target.transform.position.z);

            target.isDead();

            if (pv.IsMine)
                killCooldown = GameSystem.Instance.killCooldown;

            //pv.RPC("InstantiateRoomObject", RpcTarget.MasterClient, "Deadbody", target.transform.position, 
            //    target.transform.rotation.x, target.transform.rotation.y, target.transform.rotation.z, target.playerColor);
        }
    }

    public void isDead()
    {
        pv.RPC("isRpcDead", RpcTarget.All, (int)playerType & 0x02);

        pv.RPC("InstantiateRoomObject", RpcTarget.MasterClient, "Deadbody", transform.position,
        transform.rotation.x, transform.rotation.y, transform.rotation.z, playerColor);
    }

    [PunRPC]
    public void InstantiateRoomObject(string name, Vector3 pos, float rotX, float rotY, float rotZ, float _hue)
    {
        Quaternion rot = Quaternion.Euler(rotX, rotY, rotZ);
        Deadbody obj = PhotonNetwork.InstantiateRoomObject(name, pos, rot).GetComponent<Deadbody>();

        var _deadbody = obj.GetComponent<Deadbody>();

        _deadbody.SetColor(_hue);
    }

    // 해당 플레이어에게서 실행되는 RPC 또는 자기 자신
    [PunRPC]
    public void isRpcDead(int _enum)
    {
        playerType |= PlayerType.Ghost;
        anim.SetBool("isGhost", true);

        // 자신이 죽었을 때
        if (pv.IsMine)
        {
            foreach (var player in GameSystem.Instance.controllerList)
            {
                if ((player.playerType & PlayerType.Ghost) == PlayerType.Ghost)
                    player.SetVisibility(true);
            }

            GameSystem.Instance.ChangeLightMode(PlayerType.Ghost);
        }
        // 다른 사람이 죽었을 때
        else
        {
            PlayerController myController = null;

            foreach(var player in GameSystem.Instance.controllerList) 
            { 
                if(player.nickName.Equals(PhotonNetwork.NickName))
                {
                    myController = player;
                    break;
                }
            }

            if(myController != null)
            {
                if (((int)playerType & 0x02) != _enum)
                {
                    // 다른 사람이 죽었을 때 자신이 죽은 상태가 아니라면
                    if ((myController.playerType & PlayerType.Ghost) != PlayerType.Ghost)
                        SetVisibility(false);
                    // 다른 사람이 죽었을 때 자신이 죽은 상태라면
                    else
                        SetVisibility(true);
                }
            }
            
        }

        var collider = GetComponent<BoxCollider2D>();
        if(collider != null)
        {
            collider.enabled = false;
        }
    }

    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.6f);
        }
        else
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        }
    }

    #endregion

    public void Report()
    {
        if (GameSystem.Instance == null)
            return;

        GameSystem.Instance.StartReportMeeting(deadbodyColor);
        pv.RPC("IsReporter", RpcTarget.All, true);
    }

    #region Set

    public void SetColor(float _hue)
    {
        if (pv.IsMine)
        {
            pv.RPC("SetRPCColor", RpcTarget.AllBuffered, _hue);
            pv.RPC("RpcInitBtns", RpcTarget.All);
        }
    }

    private void SetInitColor()
    {
        if (GameSystem.Instance == null)
            return;

        if (CustomManager.Instance == null)
            return;

        var players = GameSystem.Instance.GetPlayerList();

        List<float> playerColors = new List<float>();

        for(int i = 0; i < players.Count; i++)
        {
            if (!players[i].nickName.Equals(PhotonNetwork.NickName))
            {
                playerColors.Add(players[i].playerColor);
            }
        }

        foreach (var btn in CustomManager.Instance.colorBtns)
        {
            bool isFind = false;

            foreach (var playerColor in playerColors)
            {
                if(btn.hue == playerColor)
                {
                    isFind = true;
                    break;
                }
            }

            if (!isFind)
            {
                SetColor(btn.hue);
                CustomManager.Instance.customizeUI.SetHue(playerColor);
                break;
            }
        }

    }

    #endregion

    #region Get

    public float GetColor()
    {
        return this.playerColor;
    }

    #endregion
}
