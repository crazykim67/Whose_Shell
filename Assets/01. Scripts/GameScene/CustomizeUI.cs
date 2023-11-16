using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class CustomizeUI : MonoBehaviour
{
    public GameObject customUI;

    public GameObject dimObj;

    public Button confirmBtn;

    [Header("Player")]
    public PlayerController player;

    [Header("Character Image")]
    public Image characterImage;

    private void Awake()
    {
        Material mat = new Material(characterImage.material);
        characterImage.material = mat;

        confirmBtn.onClick.AddListener(() =>
        {
            OnHide();
            OnConfirm();
        });
    }

    #region Show & Hide

    public void OnShow()
    {
        if (player == null)
            return;

        var _hue = player.mat.GetFloat("_Hue");
        characterImage.material.SetFloat("_Hue", _hue);

        player.isUI = true;
        dimObj.SetActive(true);
        customUI.SetActive(true);
    }

    public void OnHide()
    {
        player.isUI = false;
        dimObj.SetActive(false);
        customUI.SetActive(false);
    }

    #endregion

    public void OnClick(float _hue)
    {
        if (player == null)
            return;

        if (player.mat.GetFloat("_Hue") == _hue)
            return;

        characterImage.material.SetFloat("_Hue", _hue);
    }

    public void OnConfirm()
    {
        if(player == null) 
            return;

        player.SetColor(characterImage.material.GetFloat("_Hue"));
    }
}
