using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public enum PlayerType
{
    Turtle,
    Terrapin,
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

    public void SetColor(float _hue)
    {
        if (pv.IsMine)
        {
            pv.RPC("SetRPCColor", RpcTarget.AllBuffered, _hue);
        }
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

    #endregion

    #region Kill

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
            var manager = GameManager.Instance;

            //GameObject obj = GameSystem.Instance.masterClient.InstantiateRoomObject("Deadbody", target.transform.position, target.transform.rotation);
            pv.RPC("InstantiateRoomObject", RpcTarget.MasterClient, "Deadbody", target.transform.position, 
                target.transform.rotation.x, target.transform.rotation.y, target.transform.rotation.z, target.playerColor);

            if(pv.IsMine)
                killCooldown = GameSystem.Instance.killCooldown;
        }
    }

    [PunRPC]
    public void InstantiateRoomObject(string name, Vector3 pos, float rotX, float rotY, float rotZ, float _hue)
    {
        Quaternion rot = Quaternion.Euler(rotX, rotY, rotZ);
        Deadbody obj = PhotonNetwork.InstantiateRoomObject(name, pos, rot).GetComponent<Deadbody>();

        var _deadbody = obj.GetComponent<Deadbody>();

        _deadbody.SetColor(_hue);
    }

    #endregion
}
