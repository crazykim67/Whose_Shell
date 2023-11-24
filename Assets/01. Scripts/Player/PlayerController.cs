using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform spriteTr;

    private PhotonView pv;

    private float moveX, moveY;

    public float speed = 3f;

    private Camera cam;

    public SpriteRenderer sprite;
    public Material mat;

    public Animator anim;
    private bool isMove = false;

    public bool isUI = false;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            cam = Camera.main.GetComponent<Camera>();
            cam.transform.position = new Vector3(0, 0, -1);

            Material playerMat = new Material(mat);
            sprite.material = playerMat;
            mat = playerMat;
        }
    }

    private void Start()
    {
        if(pv.IsMine)
            CustomManager.Instance.SetPlayer(this);
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
    }

    #endregion
}
