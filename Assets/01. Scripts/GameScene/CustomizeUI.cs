using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class CustomizeUI : MonoBehaviour
{
    public float hue;

    [Header("Character Image")]
    public Image characterImage;

    private void Awake()
    {
        Material mat = new Material(characterImage.material);
        characterImage.material = mat;
    }

    #region Show & Hide

    public void OnShow()
    {
        gameObject.SetActive(true);

        characterImage.material.SetFloat("_Hue", hue);

        CustomManager.Instance.player.OnStop();
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    #endregion

    public void OnClick(float _hue)
    {
        characterImage.material.SetFloat("_Hue", _hue);
    }

    public void OnConfirm()
    {
        CustomManager.Instance.player?.SetColor(characterImage.material.GetFloat("_Hue"));
        hue = characterImage.material.GetFloat("_Hue");
    }

    public void SetHue(float _hue)
    {
        this.hue = _hue; 
    }
}
