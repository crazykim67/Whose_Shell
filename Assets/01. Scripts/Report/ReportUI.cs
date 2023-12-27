using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ReportUI : MonoBehaviour
{
    [SerializeField]
    private Image deadbodyImage;

    [SerializeField]
    private Material mat;

    public void OnShow(float playerColor)
    {
        foreach(var player in GameSystem.Instance.controllerList)
        {
            if (player.nickName.Equals(PhotonNetwork.NickName))
            {
                player.isUI = true;
                break;
            }
        }

        Material _mat = Instantiate(mat);
        deadbodyImage.material = _mat;

        gameObject.SetActive(true);

        deadbodyImage.material.SetFloat("_Hue", playerColor);
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }
}
