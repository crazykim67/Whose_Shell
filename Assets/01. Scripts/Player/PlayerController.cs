using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Rendering.Universal;

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
    }

    #endregion

    #region TrrigerEnter / Exit

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameSystem.Instance == null)
            return;

        if (!GameSystem.Instance.isStart)
            return;

        if(coll.tag == "Player")
        {
            PlayerController otherPlayer = coll.gameObject.GetComponent<PlayerController>();

            if (otherPlayer.pv.IsMine)
                return;

            otherPlayer.playerSet.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (GameSystem.Instance == null)
            return;

        if (!GameSystem.Instance.isStart)
            return;

        if (coll.tag == "Player")
        {
            PlayerController otherPlayer = coll.gameObject.GetComponent<PlayerController>();

            if (otherPlayer.pv.IsMine)
                return;

            otherPlayer.playerSet.SetActive(false);
        }
    }

    #endregion

    public void SetKillButton()
    {
        if (InGameUIManager.Instance == null)
            return;

        if(playerType == PlayerType.Terrapin)
            InGameUIManager.Instance.KillUI.OnShow();
    }
}
