using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIState
{
    None,
    Room,
    Color,
}

public class CustomManager : MonoBehaviour
{
    #region Instance

    private static CustomManager instance;

    public static CustomManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new CustomManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    public GameRuleSetting gameRuleSetting;
    public CustomizeUI customizeUI;

    public GameObject panel;
    public GameObject dim;

    public Button customBtn;

    public Button colorBtn;
    public Button ruleBtn;

    public Button confirmBtn;

    //[HideInInspector]
    public PlayerController player;

    public List<ColorButton> colorBtns = new List<ColorButton>();

    private void Awake()
    {
        instance = this;

        customBtn.onClick.AddListener(() => { OnShow(UIState.None); });
        colorBtn.onClick.AddListener(() => { OnShow(UIState.Color); });
        ruleBtn.onClick.AddListener(() => { OnShow(UIState.Room); });
        confirmBtn.onClick.AddListener(OnHide);

        gameRuleSetting.GetPhotonView();
        OnHide();
    }

    public void OnShow(UIState state)
    {
        InitBtns();
        player.isUI = true;

        switch(state)
        {
            case UIState.None:
                {
                    panel.SetActive(true);
                    dim.SetActive(true);

                    colorBtn.gameObject.SetActive(false);
                    ruleBtn.gameObject.SetActive(true);

                    customizeUI.OnShow();
                    gameRuleSetting.OnHide();
                    break;
                }
            case UIState.Color:
                {
                    colorBtn.gameObject.SetActive(false);
                    ruleBtn.gameObject.SetActive(true);

                    gameRuleSetting.OnHide();
                    customizeUI.OnShow();
                    break;
                }
            case UIState.Room:
                {
                    colorBtn.gameObject.SetActive(true);
                    ruleBtn.gameObject.SetActive(false);

                    gameRuleSetting.OnShow();
                    customizeUI.OnHide();
                    break;
                }
        }
    }

    public void OnHide()
    {
        if(player != null)
            player.isUI = false;

        panel.SetActive(false);
        dim.SetActive(false);

        customizeUI.OnConfirm();
        gameRuleSetting.OnConfirm();
    }

    public void SetPlayer(PlayerController _player)
    {
        this.player = _player;
    }

    public void SetActive(bool isAct)
    {
        this.gameObject.SetActive(isAct);
    }

    public void InitBtns()
    {
        if (GameSystem.Instance == null)
            return;

        var players = GameSystem.Instance.GetPlayerList();

        for (int i = 0; i < colorBtns.Count; i++)
            colorBtns[i].SetInteractable(true);

        for(int i = 0; i < players.Count; i++)
        {
            foreach(var btn in colorBtns)
            {
                if (players[i].playerColor == btn.hue)
                {
                    btn.SetInteractable(false);
                    break;
                }
            }
        }
    }
}
