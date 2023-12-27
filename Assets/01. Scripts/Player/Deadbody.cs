using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Deadbody : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private float shellColor;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        Material _mat = Instantiate(sprite.material);
        sprite.material = _mat;
    }

    public void SetColor(float _hue)
    {
        pv.RPC("SetRpcColor", RpcTarget.All, _hue);
    }

    [PunRPC]
    private void SetRpcColor(float _hue)
    {
        sprite.material.SetFloat("_Hue", _hue);
        shellColor = _hue;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var player = coll.GetComponent<PlayerController>();
        if(player != null && player.pv.IsMine && (player.playerType & PlayerType.Ghost) != PlayerType.Ghost)
        {
            InGameUIManager.Instance.ReportButtonUI.SetInteractable(true);
            player.deadbodyColor = shellColor;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        var player = coll.GetComponent<PlayerController>();
        if (player != null && player.pv.IsMine && (player.playerType & PlayerType.Ghost) != PlayerType.Ghost)
        {
            InGameUIManager.Instance.ReportButtonUI.SetInteractable(false);
        }
    }
}
